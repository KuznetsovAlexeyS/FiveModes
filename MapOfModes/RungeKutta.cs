using System;
using DotNumerics.ODE;

namespace MapOfModes
{
	public partial class ExplicitRungeKutta
	{
		private OdeExplicitRungeKutta45 odeRK = new OdeExplicitRungeKutta45();

		public double Pr { get; } // число Прантдля
		public double nu { get; } // частота электрического поля
		public double e { get; } // электрическое число Рэлея
		public double r { get; } // тепловое число Рэлея
		private double k; // волновое число
		private double b;
		private double d;
		private int tStart;
		private int tEnd;
		private int iterationsInOneSecond;

		public ExplicitRungeKutta(double Pr, double nu, double e, double r, double k, int tStart, int tEnd, int iterationsInOneSecond)
		{
			this.Pr = Pr;
			this.nu = nu;
			this.e = e;
			this.r = r;
			this.k = k;
			this.b = 4 / (1 + k * k);
			this.d = (4 + k * k) / (1 + k * k);
			if (tStart >= tEnd || tEnd > 1000) throw new ArgumentException(); // Начальное время не может быть больше конечного времени, 
			//также, по техническим ограничениям, конечное время не может быть больше 1000. Если хотите больше -- организуйте запись в файл 
			//с последующим чтением или создайте несколько массивов.
			this.tStart = tStart;
			this.tEnd = tEnd;
			this.iterationsInOneSecond = iterationsInOneSecond;
		}

		public double[] Solve(int numberOfCountedMod)
		{
			OdeFunction fun = new OdeFunction(ODEs);
			double[] y0 = new double[5];
			y0[0] = 0.0; // Начальные условия
			y0[1] = 0.5;
			y0[2] = 0.0;
			y0[3] = 0.0;
			y0[4] = 0.0;
			this.odeRK.InitializeODEs(fun, 5);
			double[,] sol = odeRK.Solve(y0, 0, 1.0/iterationsInOneSecond, tEnd); // iterationsInOneSeconds -- величина, обратная шагу.
			// tEnd -- время, до которого считаем.
			double[] mod = new double [sol.GetLength(0) - tStart * iterationsInOneSecond]; // Отсекаем всё до момента времени tStart.
			int amountOfPoints = sol.GetLength(0);
			for(int i=tStart* iterationsInOneSecond; i<amountOfPoints; i++)
			{
				mod[i - tStart*iterationsInOneSecond] = sol[i, numberOfCountedMod];
			}
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
		}
	}
}