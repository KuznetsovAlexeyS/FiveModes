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
		private volatile bool canceled;
		private volatile bool paused;
		private readonly PointPairList fadingCountedPoints;
		private readonly PointPairList quasiPeriodicCountedPoints;
		private readonly PointPairList chaosCountedPoints;
		private readonly PointPairList somethingUnknownCountedPoints;
		private Thread firstThread;
		private Thread secondThread;
		private Thread thirdThread;
		private Thread fourthThread;

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
			chart.GraphPane.Title.Text = "";
			chart.GraphPane.XAxis.Title.Text = "nu";
			chart.GraphPane.YAxis.Title.Text = "e";
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
			firstThread = new Thread(() =>
			{
				try
				{
					for (double eStep = 0; eStep < 2.5; eStep += 0.05)
					{
						for (double nuStep = 0.0; nuStep < 0.01; nuStep += 0.001)
						{
							var info = new ExplicitRungeKutta(100, 0.05 + nuStep, 67.5 + eStep, 0, 0.962, 470, 1000, 1000);
							var mode = ModeGetter.GetMode(info.Solve(1));
							var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = info.nu, Y = info.e }, mode)));
						}
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			secondThread = new Thread(() =>
			{
				try
				{
					for (double eStep = 0; eStep < 2.5; eStep += 0.05)
					{
						for (double nuStep = 0.0; nuStep < 0.01; nuStep += 0.001)
						{
							var info = new ExplicitRungeKutta(100, 0.05 + nuStep, 70.0 + eStep, 0, 0.962, 470, 1000, 1000);
							var mode = ModeGetter.GetMode(info.Solve(1));
							var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = info.nu, Y = info.e }, mode)));
						}
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			thirdThread = new Thread(() =>
			{
				try
				{
					for (double eStep = 0; eStep < 2.5; eStep += 0.05)
					{
						for (double nuStep = 0.0; nuStep < 0.01; nuStep += 0.001)
						{
							var info = new ExplicitRungeKutta(100, 0.05 + nuStep, 72.5 + eStep, 0, 0.962, 470, 1000, 1000);
							var mode = ModeGetter.GetMode(info.Solve(1));
							var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = info.nu, Y = info.e }, mode)));
						}
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			fourthThread = new Thread(() =>
			{
				try
				{
					for (double eStep = 0; eStep < 2.5; eStep += 0.05)
					{
						for (double nuStep = 0.0; nuStep < 0.01; nuStep += 0.001)
						{
							var info = new ExplicitRungeKutta(100, 0.05 + nuStep, 75.0 + eStep, 0, 0.962, 470, 1000, 1000);
							var mode = ModeGetter.GetMode(info.Solve(1));
							var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = info.nu, Y = info.e }, mode)));
						}
					}
				}

				catch
				{
					throw new Exception();
				}
			});

			firstThread.Start();
			secondThread.Start();
			thirdThread.Start();
			fourthThread.Start();
		}

		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			Application.Run(form);
		}

		private void AddPoint(DataPoint p, Mode mode)
		{
			switch (mode)
			{
				case Mode.Fading:
					fadingCountedPoints.Add(p.X, p.Y);
					break;
				case Mode.QuasiPeriodic:
					quasiPeriodicCountedPoints.Add(p.X, p.Y);
					break;
				case Mode.Chaos:
					chaosCountedPoints.Add(p.X, p.Y);
					break;
				case Mode.SomethingUnknown:
					somethingUnknownCountedPoints.Add(p.X, p.Y);
					break;
			}
			chart.AxisChange();
			chart.Invalidate();
			chart.Refresh();
		}
	}
}
