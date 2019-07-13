using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
using System.Collections.Generic;
using System.Text;

namespace MapOfModes
{
	class Program
	{
		private readonly CustomForm mainForm;
		private readonly SettingForm settingForm;
		private readonly ZedGraphControl chart;
		private volatile bool paused = false;
		private volatile bool canceled = false;
		private volatile bool countingStarted = false;
		private volatile bool dataIsCorrect = true;
		private PointPairList fadingCountedPoints;
		private PointPairList quasiPeriodicCountedPoints;
		private PointPairList chaosCountedPoints;
		private PointPairList somethingUnknownCountedPoints;
		private Thread firstThread;
		private volatile List<StringBuilder> saved;

		public Program()
		{
			saved = new List<StringBuilder>();

			mainForm = new CustomForm
			{
				WindowState = FormWindowState.Maximized,
				Text = "Карта режимов"
			};
			chart = new ZedGraphControl()
			{
				Location = new Point(0, 0),
				Dock = DockStyle.Fill,
			};
			chart.GraphPane.Title.Text = "Карта режимов";
			chart.GraphPane.XAxis.Title.Text = "...";
			chart.GraphPane.YAxis.Title.Text = "...";

			fadingCountedPoints = new PointPairList();
			quasiPeriodicCountedPoints = new PointPairList();
			chaosCountedPoints = new PointPairList();
			somethingUnknownCountedPoints = new PointPairList();

			var fading = chart.GraphPane.AddCurve("Затухание", fadingCountedPoints, Color.Green, SymbolType.Diamond);// 
			var quasiPeriodic = chart.GraphPane.AddCurve("Квазипериодический режим", quasiPeriodicCountedPoints, Color.Blue, SymbolType.Triangle);
			var chaos = chart.GraphPane.AddCurve("Хаос", chaosCountedPoints, Color.Red, SymbolType.Star);
			var somethingUnknown = chart.GraphPane.AddCurve("Неизвестный режим", somethingUnknownCountedPoints, Color.Gray, SymbolType.XCross);

			fading.Line.IsVisible = false;//
			quasiPeriodic.Line.IsVisible = false;
			chaos.Line.IsVisible = false;
			somethingUnknown.Line.IsVisible = false;

			chart.GraphPane.XAxis.MajorGrid.IsVisible = true;
			chart.GraphPane.YAxis.MajorGrid.IsVisible = true;

			mainForm.Controls.Add(chart);

			mainForm.pause.Click += (sender, args) =>
			{
				paused = !paused;
			};
			mainForm.save.Click += (sender, args) => Save();
			mainForm.read.Click += (sender, args) => Read();
		
			settingForm = new SettingForm { };
			mainForm.openSettings.Click += (sender, args) => settingForm.Show();
			settingForm.accept.Click += OnAcception;
			settingForm.accept.Click += (sender, args) => 
			{
				if (dataIsCorrect)
					settingForm.Hide();
			};
			FormClosingEventHandler warning = (sender, args) =>
			{
				if (countingStarted)
				{
					bool tempPaused = false;
					if (paused == true) tempPaused = true;
					paused = true;
					var res = MessageBox.Show("Расчёт ещё идёт, действительно закрыть приложение?", "Предупреждение", MessageBoxButtons.YesNo);
					if (res == DialogResult.Yes)
					{
						args.Cancel = false;
						canceled = true;
					}
					else args.Cancel = true;
					paused = tempPaused;
				}
			};
			mainForm.FormClosing += warning;
		}

		private void OnAcception(object sender, EventArgs e)
		{
			ParserToDouble Pr = new ParserToDouble(settingForm.PrStart.Text);
			ParserToDouble el = new ParserToDouble(settingForm.eStart.Text);
			ParserToDouble r = new ParserToDouble(settingForm.rStart.Text);
			ParserToDouble nu = new ParserToDouble(settingForm.nuStart.Text);
			ParserToDouble k = new ParserToDouble(settingForm.kStart.Text);
			ParserToDouble startX = new ParserToDouble(settingForm.startModeX.Text);
			ParserToDouble startY = new ParserToDouble(settingForm.startModeY.Text);
			ParserToDouble startZ = new ParserToDouble(settingForm.startModeZ.Text);
			ParserToDouble startV = new ParserToDouble(settingForm.startModeV.Text);
			ParserToDouble startW = new ParserToDouble(settingForm.startModeW.Text);
			ParserToDouble horizontalValueStart = new ParserToDouble(settingForm.horizontalValueStart.Text);
			ParserToDouble verticalValueStart = new ParserToDouble(settingForm.verticalValueStart.Text);
			ParserToDouble horizontalValueStep = new ParserToDouble(settingForm.horizontalValueStep.Text);
			ParserToDouble verticalValueStep = new ParserToDouble(settingForm.verticalValueStep.Text);
			ParserToDouble horizontalValueEnd = new ParserToDouble(settingForm.horizontalValueEnd.Text);
			ParserToDouble verticalValueEnd = new ParserToDouble(settingForm.verticalValueEnd.Text);

			if (!Pr.IsDataCorrect || !el.IsDataCorrect || !r.IsDataCorrect || !nu.IsDataCorrect || !k.IsDataCorrect
				|| !startX.IsDataCorrect || !startY.IsDataCorrect || !startZ.IsDataCorrect || !startV.IsDataCorrect || !startW.IsDataCorrect
				|| !horizontalValueStart.IsDataCorrect || !verticalValueStart.IsDataCorrect
				|| !horizontalValueStep.IsDataCorrect || !verticalValueStart.IsDataCorrect
				|| !horizontalValueEnd.IsDataCorrect || !verticalValueEnd.IsDataCorrect)
			{
				dataIsCorrect = false;
				MessageBox.Show("Введённые данные некорректны");
				return;
			}
			else dataIsCorrect = true;

			var horizontalParameter = ParserToEnums.ParseToParameter(settingForm.horizontalParameter.SelectedItem.ToString());
			var verticalParameter = ParserToEnums.ParseToParameter(settingForm.verticalParameter.SelectedItem.ToString());
			var mode = ParserToEnums.ParseToMode(settingForm.mode.SelectedItem.ToString());
			var CBMV = settingForm.continuationByMode.Checked;

			SubscribeAxis(mode, horizontalParameter, verticalParameter);

			var title = new StringBuilder();
			title.Append(mode.ToString());
			title.Append(" ");
			title.Append(horizontalParameter.ToString());
			title.Append(" ");
			title.Append(verticalParameter.ToString());
			saved.Add(title);

			firstThread = new Thread(() =>
			{
				try
				{
					countingStarted = true;
					var system = new ODESystem(Pr.Value, nu.Value, el.Value, r.Value, k.Value,
						startX.Value, startY.Value, startZ.Value, startV.Value, startW.Value,
						470, 1000, 1000);
					foreach(var point in ModeGetter.GoThroughValuesAndGetMode(system, 
						horizontalValueStart.Value, horizontalValueStep.Value, horizontalValueEnd.Value, 
						verticalValueStart.Value, verticalValueStep.Value, verticalValueEnd.Value, 
						470, 1000, 1000, 
						startX.Value, startY.Value, startZ.Value, startV.Value, startW.Value, CBMV,
						horizontalParameter, verticalParameter, mode))
					{
						var invokation = mainForm.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = point.HorizontalAxis, Y = point.VerticalAxis }, point.Regime)));
						var infoString = new StringBuilder();

						infoString.Append(point.HorizontalAxis.ToString());
						infoString.Append(" ");
						infoString.Append(point.VerticalAxis.ToString());
						infoString.Append(" ");
						infoString.Append(point.Regime.ToString());
						saved.Add(infoString);

						while (paused) Thread.Sleep(20);
						if (canceled) return;
					}
					countingStarted = false;
				}

				catch
				{
					throw new ArgumentException();
				}
			});
			firstThread.Start();
		}

		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			Application.Run(mainForm);
		}

		private void AddPoint(DataPoint p, Regime mode)
		{
			switch (mode)
			{
				case Regime.Fading:
					fadingCountedPoints.Add(p.X, p.Y);
					break;
				case Regime.QuasiPeriodic:
					quasiPeriodicCountedPoints.Add(p.X, p.Y);
					break;
				case Regime.Chaos:
					chaosCountedPoints.Add(p.X, p.Y);
					break;
				case Regime.SomethingUnknown:
					somethingUnknownCountedPoints.Add(p.X, p.Y);
					break;
			}
			chart.AxisChange();
			chart.Invalidate();
			chart.Refresh();
		}

		private void Save()
		{
			var savedStrings = new string[saved.Count];
			for(int i=0; i<saved.Count; i++)
			{
				savedStrings[i] = saved[i].ToString();
			}
			File.WriteAllLines("SavedMap.txt", savedStrings);
		}
		
		private void Read()
		{
			string[] data = File.ReadAllLines("SavedMap.txt");
			var parameters = data[0].Split(' ');

			var mode = ParserToEnums.ParseToMode(parameters[0]);
			var horizontal = ParserToEnums.ParseToParameter(parameters[1]);
			var vertical = ParserToEnums.ParseToParameter(parameters[2]);

			var cutData = data.Skip(1);

			SubscribeAxis(mode, horizontal, vertical);

			foreach(var point in cutData)
			{
				var splitedString = point.Split(' ');

				var horizontalValue = Double.Parse(splitedString[0]);
				var verticalValue = Double.Parse(splitedString[1]);
				var regime = ParserToEnums.ParseToRegime(splitedString[2]);

				var invokation = mainForm.BeginInvoke((Action)(() => 
				AddPoint(new DataPoint { X = horizontalValue, Y = verticalValue }, regime)));
			}

		}

		private void SubscribeAxis(Mode mode, Parameter horizontal, Parameter vertical)
		{
			chart.GraphPane.Title.Text = "Карта режимов для моды " + mode.ToString();
			chart.GraphPane.XAxis.Title.Text = horizontal.ToString();
			chart.GraphPane.YAxis.Title.Text = vertical.ToString();
		}
	}
}
