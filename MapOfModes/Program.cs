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
		private readonly PointPairList countedPoints;
		private Thread firstThread;
		//private Thread secondThread;
		//private Thread thirdThread;
		//private Thread fourthThread;

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
			chart.GraphPane.Title.Text = "Фурье-разложение";
			chart.GraphPane.XAxis.Title.Text = "Частота";
			chart.GraphPane.YAxis.Title.Text = "Амплитда";
			countedPoints = new PointPairList();
			var original = chart.GraphPane.AddCurve("value", countedPoints, Color.Blue, SymbolType.None);
			original.Line.IsVisible = true;
			form.Controls.Add(chart);
			chart.KeyDown += (sender, args) => paused = !paused;
			form.FormClosing += (sender, args) => { canceled = true; };
			form.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs e)
		{
			firstThread = new Thread(() =>
			{
				var tempSys = new ExplicitRungeKutta(100, 0.075, 69.3, 0, 0.962, 470, 1000, 1000); // tStart -- время, начиная с которого мы берём данные.
				//Расчёт для значений 0-tStart всё равно пройдёт.
				//Если берёте разницу между tStart и tEnd больше 530 или меняете количество итераций в секунде, не забудьте изменить nPoints в FFT для корректной работы.
				var step = 0;
				try
				{
				foreach (var point in FFT.GetFFT(tempSys.Solve(1)).Take(3000).ToArray()) // Аргумент в Solve для моды X -- 1, Y -- 2, Z -- 3, V -- 4, W -- 5. 
					{
						if (canceled) return;
						var j = step;
						var invokation = form.BeginInvoke((Action)(() => AddPoint(new DataPoint { X = j / 524.288, Y = point })));
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
