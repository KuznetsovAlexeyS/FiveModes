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
		private readonly Form form;
		private readonly ZedGraphControl chart;
		private volatile bool canceled = false;
		private volatile bool paused;
		private PointPairList fadingCountedPoints;
		private PointPairList quasiPeriodicCountedPoints;
		private PointPairList chaosCountedPoints;
		private PointPairList somethingUnknownCountedPoints;
		private Thread firstThread;
		private Thread secondThread;

		public Program()
		{
			form = new Form
			{
				WindowState = FormWindowState.Maximized,
				Text = "Program"
			};
			chart = new ZedGraphControl()
			{
				Dock = DockStyle.Fill, // говорим графику заполнить всё окно.
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
			form.Controls.Add(chart);
			chart.KeyDown += (sender, args) => paused = !paused;
			form.FormClosing += (sender, args) => { canceled = true; };
			form.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs e)
		{
			firstThread = new Thread(() => // ПЕРВЫЙ ТРЕД. ДОБАВИТЬ ОТДЕЛЬНЫЙ КЛАСС, ВОЗВРАЩАЮЩИЙ ЧЕРЕЗ YIELD RETURN ЗНАЧЕНИЯ ModelCheck
			{
				try
				{
					var system = new ODESystem(100, 0.05, 67.0, 0, 0.962, 470, 1000, 1000);
					foreach(var point in ModeGetter.GoThroughValuesAndGetMode(system, 0.045, 0.0005, 0.050, 67.0, 0.05, 70, 470, 1000, 1000, Parameter.nu, Parameter.e, Mode.X))
					{
						var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = point.HorizontalAxis, Y = point.VerticalAxis }, point.Regime)));
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			secondThread = new Thread(() => // ВТОРОЙ ТРЕД
			{
				try
				{
					var system = new ODESystem(100, 0.05, 67.0, 0, 0.962, 470, 1000, 1000);
					foreach (var point in ModeGetter.GoThroughValuesAndGetMode(system, 0.050, 0.0005, 0.055, 67.0, 0.05, 70, 470, 1000, 1000, Parameter.nu, Parameter.e, Mode.X))
					{
						var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = point.HorizontalAxis, Y = point.VerticalAxis }, point.Regime)));
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			firstThread.Start();
			secondThread.Start();
		}

		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			Application.Run(form);
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
