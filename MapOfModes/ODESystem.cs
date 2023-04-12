using System;
using DotNumerics.ODE;

namespace MapOfModes
{
	public partial class ODESystem
	{

		public double Pr { get; } // число Прантдля
		public double nu { get; } // частота электрического поля
		public double e { get; } // электрическое число Рэлея
		public double r { get; } // тепловое число Рэлея
		public double k { get; } // волновое число
		private double b;
		private double d;
		private int tStart;
		private int tEnd;
		private int iterationsInOneSecond;
		private double startX;
		private double startY;
		private double startZ;
		private double startV;
		private double startW;

		public ODESystem(double Pr, double nu, double e, double r, double k,
			double startX, double startY, double startZ, double startV, double startW,
			int tStart, int tEnd, int iterationsInOneSecond)
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

			this.startX = startX;
			this.startY = startY;
			this.startZ = startZ;
			this.startV = startV;
			this.startW = startW;
		}

		private OdeExplicitRungeKutta45 odeRK = new OdeExplicitRungeKutta45();

        public double[] Solve(int numberOfCountedMod)
        {
            double[] startValues = new double[5];
            startValues[0] = startX; // Начальные условия
            startValues[1] = startY;
            startValues[2] = startZ;
            startValues[3] = startV;
            startValues[4] = startW;

            try // Решает быстро, но может выплюнуть исключение.
            {
                OdeFunction fun = new OdeFunction(ODEs);
                double[] y0 = new double[5];
                y0[0] = startX;
                y0[1] = startY;
                y0[2] = startZ;
                y0[3] = startV;
                y0[4] = startW;
                this.odeRK.InitializeODEs(fun, 5);
                double[,] sol = odeRK.Solve(y0, 0, 1.0 / iterationsInOneSecond, tEnd);

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

            catch // Решает где-то в 10 раз медленнее, но стабильно.
            {
                RungeKuttaSolver solver = new RungeKuttaSolver(
                    startValues, 0, 1000000, 0.001, 0.000001,
                    (double[] y, double t) =>
                    {
                        double[] yprime = new double[5];
                        double cos = Math.Cos(2 * Math.PI * nu * t);
                        yprime[0] = Pr * (-y[0] + r * y[1] + e * y[4] * cos * cos); // Система уравнений
                        yprime[1] = -y[1] + y[0] + y[0] * y[2];
                        yprime[2] = -b * y[2] - y[0] * y[1];
                        yprime[3] = Pr * (-d * y[3] + (r * y[4] - e * y[1] * cos * cos) / d);
                        yprime[4] = -d * y[4] + y[3];
                        return yprime;
                    });

                var sol = solver.Solve();

                double[] mod = new double[sol.GetLength(0) - tStart * iterationsInOneSecond]; // Отсекаем всё до момента времени tStart.
                int amountOfPoints = sol.GetLength(0);
                for (int i = tStart * iterationsInOneSecond; i < amountOfPoints; i++)
                {
                    mod[i - tStart * iterationsInOneSecond] = sol[i][numberOfCountedMod];
                }
                GlobalModes.X = sol[amountOfPoints - 1][1]; // На случай продолжения по параметру. 
                GlobalModes.Y = sol[amountOfPoints - 1][2]; // Можно оптимизировать, однако 5 присвоений не сильно затратны, 
                GlobalModes.Z = sol[amountOfPoints - 1][3]; // Зато позволяют сократить количество переменных, которые необходимо передовать в ODESystem
                GlobalModes.V = sol[amountOfPoints - 1][4];
                GlobalModes.W = sol[amountOfPoints - 1][5];
                return mod;
            }
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