using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace MapOfModes
{
	class Program
	{
		private readonly Form form;
		private readonly ZedGraphControl chart;
		private readonly PointPairList countedPoints;
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
			chart.GraphPane.Title.Text = "Карта режимов";
			chart.GraphPane.XAxis.Title.Text = "t";
			chart.GraphPane.YAxis.Title.Text = "X";
			countedPoints = new PointPairList();
			var original = chart.GraphPane.AddCurve("value", countedPoints, Color.Blue, SymbolType.None);
			original.Line.IsVisible = true;
			form.Controls.Add(chart);
			form.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs e)
		{
			firstThread = new Thread(() =>
			{
				var tempSys = new ExplicitRungeKutta(100, 0.05, 67.85, 0, 0.962, 980, 999); // tStart -- время, начиная с которого мы берём данные.
				//Расчёт для значений 0-tStart всё равно пройдёт.
				var step = 0;
				try
				{
					foreach(var point in tempSys.Solve(1)) // Аргумент в Solve для моды X -- 1, Y -- 2, Z -- 3, V -- 4, W -- 5. 
					{
						var j = step;
						var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = j, Y = point })));
						step++;
					}
				}
				catch
				{
					throw new Exception();
				}
			});

			/*secondThread = new Thread(() =>
			{
				try
				{
					foreach (var point in Tester.Test(new Random(), -100, -80, -10, 10)) // Сделать Рунге-Кутту
					{
						var invokation = form.BeginInvoke((Action)(() => AddPoint(point)));
						Thread.Sleep(300);
					}
				}
				catch
				{
					// Если это убрать, то будет ошибка при закрытии формы. 
					//Если хочется -- можно добавить сюда что-нибудь, связанное с эксепшенами.
				}
			});*/

			firstThread.Start();
			//secondThread.Start();
		}

		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			Application.Run(form);
		}

		private void AddPoint(DataPoint p)
		{
			countedPoints.Add(p.X, p.Y);
			chart.AxisChange();
			chart.Invalidate();
			chart.Refresh();
		}
	}
}
