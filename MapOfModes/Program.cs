﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

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

		public Program()
		{
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
			chart.GraphPane.Title.Text = "...";
			chart.GraphPane.XAxis.Title.Text = "..."; // НЕ ЗАБЫТЬ СДЕЛАТЬ ТАК, ЧТОБЫ ОНИ МЕНЯЛИСЬ ПРИ ИЗМЕНЕНИИ ПАРАМЕТРОВ ВЫЗОВА GoThroughValuesAndGetMode
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
				bool tempPaused = false;
				if (paused == true) tempPaused = true;
				paused = true;
				var res = MessageBox.Show("Карта не сохранена, действительно закрыть приложение?", "Предупреждение", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					args.Cancel = false;
					canceled = true;
				}
				else args.Cancel = true;
				paused = tempPaused;
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
			ParserToDouble horizontalValueEnd = new ParserToDouble(settingForm.horizontalValueEnd.Text);
			ParserToDouble verticalValueEnd = new ParserToDouble(settingForm.verticalValueEnd.Text);
			if (!Pr.IsDataCorrect || !el.IsDataCorrect || !r.IsDataCorrect || !nu.IsDataCorrect || !k.IsDataCorrect
				|| !startX.IsDataCorrect || !startY.IsDataCorrect || !startZ.IsDataCorrect || !startV.IsDataCorrect || !startW.IsDataCorrect
				|| !horizontalValueEnd.IsDataCorrect || !verticalValueEnd.IsDataCorrect)
			{
				dataIsCorrect = false;
				MessageBox.Show("Введённые данные некорректны");
				return;
			}
			else dataIsCorrect = true;

			var horizontalParameter = ParserToEnums.ParseToParameter(settingForm.horizontalValue.SelectedItem.ToString());
			var verticalParameter = ParserToEnums.ParseToParameter(settingForm.verticalValue.SelectedItem.ToString());
			var mode = ParserToEnums.ParseToMode(settingForm.mode.SelectedItem.ToString());
			var CBMV = settingForm.continuationByMode.Checked;

			firstThread = new Thread(() =>
			{
				try
				{
					var system = new ODESystem(Pr.Value, nu.Value, el.Value, r.Value, k.Value,
						470, 1000, 1000);
					foreach(var point in ModeGetter.GoThroughValuesAndGetMode(system, 0.05, 0.005, 0.051, 67.0, 0.1, 70.0, 470, 1000, 1000, 
						horizontalParameter, verticalParameter, mode))
					{
						var invokation = mainForm.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = point.HorizontalAxis, Y = point.VerticalAxis }, point.Regime)));
						while (paused) Thread.Sleep(20);
						if (canceled) return;
					}
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
	}
}
