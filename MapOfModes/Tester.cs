using System;
using System.Collections.Generic;
using ZedGraph;


namespace MapOfModes
{
	class Tester
	{
		public static IEnumerable<DataPoint> Test(Random rnd, int leftBorder, int rightBorder, int lowBorder, int highBorder)
		{
			while (true)
			{
				yield return new DataPoint { X = rnd.Next(leftBorder, rightBorder), Y = rnd.Next(lowBorder, highBorder) };
			}

			#region
			/*firstThread = new Thread(() =>
			{
				try
				{
					var tempSys = new ExplicitRungeKutta(100, 0.054, 73.4, 0, 0.962, 470, 1000, 1000);
					double step = 0.0;
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
			});*/

			/*private void AddPoint(DataPoint p)
			{
				fadingCountedPoints.Add(p.X, p.Y);
				chart.AxisChange();
				chart.Invalidate();
				chart.Refresh();
			}*/
			#endregion //  // Для получения картин FFT и зависимости моды от времени см. сюда

			#region
		/*public double[] Solve(int numberOfCountedMod)
		{
			OdeFunction fun = new OdeFunction(ODEs);
			double[] y0 = new double[5];
			y0[0] = startX; // Начальные условия
			y0[1] = startY;
			y0[2] = startZ;
			y0[3] = startV;
			y0[4] = startW;
			this.odeRK.InitializeODEs(fun, 5);
			double[,] sol = odeRK.Solve(y0, 0, 1.0 / iterationsInOneSecond, tEnd); // iterationsInOneSeconds -- величина, обратная шагу.
																				   // tEnd -- время, до которого считаем.
			double[] mod = new double[sol.GetLength(0) - tStart * iterationsInOneSecond]; // Отсекаем всё до момента времени tStart.
			int amountOfPoints = sol.GetLength(0);
			for (int i = tStart * iterationsInOneSecond; i < amountOfPoints; i++)
			{
				mod[i - tStart * iterationsInOneSecond] = sol[i, numberOfCountedMod];
			}
			GlobalModes.X = sol[amountOfPoints - 1, 1]; // На случай продолжения по параметру. 
			GlobalModes.Y = sol[amountOfPoints - 1, 2]; // Можно оптимизировать, однако 5 присвоений не сильно затратны, 
			GlobalModes.Z = sol[amountOfPoints - 1, 3]; // Зато позволяют сократить количество переменных, которые необходимо передовать в ODESystem
			GlobalModes.V = sol[amountOfPoints - 1, 4];
			GlobalModes.W = sol[amountOfPoints - 1, 5];
			return mod;
		}

		double[] yprime = new double[5];

		private double[] ODEs(double t, double[] y)
		{
			yprime[0] = Pr * (-y[0] + r * y[1] + e * y[4] * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)); // Система уравнений
			yprime[1] = -y[1] + y[0] + y[0] * y[2];
			yprime[2] = -b * y[2] - y[0] * y[1];
			yprime[3] = Pr * (-d * y[3] + (r * y[4] - e * y[1] * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)) / d);
			yprime[4] = -d * y[4] + y[3];
			return this.yprime;
		}*/
		#endregion
	}
	}
}