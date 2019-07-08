using System;
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
		private volatile bool paused;
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
			mainForm.pause.Click += (sender, args) => paused = !paused;
			mainForm.Shown += OnShown;
			settingForm = new SettingForm { };
			mainForm.openSettings.Click += (sender, args) => settingForm.Show();
			/*mainForm.FormClosing += (sender, args) =>
			{
				settingForm.Close();
			};
			mainForm.FormClosed += (sender, args) =>
			{
				Application.Exit(); // Добавить окно, спрашивающее хочет ли пользователь закрывать прогу, 
				//чтобы тред успел досчитать и не выкинул ошибку
			};*/
		}

		private void OnShown(object sender, EventArgs e)
		{
			firstThread = new Thread(() =>
			{
				try
				{
					var system = new ODESystem(100, 0.05, 67.0, 0.0, 0.962, 470, 1000, 1000);
					foreach(var point in ModeGetter.GoThroughValuesAndGetMode(system, 0.05, 0.005, 0.051, 67.0, 0.1, 70.0, 470, 1000, 1000, Parameter.nu, Parameter.e, Mode.X))
					{
						var invokation = mainForm.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = point.HorizontalAxis, Y = point.VerticalAxis }, point.Regime)));
						while (paused) Thread.Sleep(20);
					}
				}

				catch
				{
					throw new Exception();
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
