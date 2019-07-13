using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;


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

		public double[] Solve(int numberOfCountedMod)
		{
			int N = iterationsInOneSecond * tEnd;
			Vector<double> y0 = Vector<double>.Build.Dense(new[] {
				GlobalModes.X, GlobalModes.Y, GlobalModes.Z, GlobalModes.V, GlobalModes.W }); // Начальные условия

			Func<double, Vector<double>, Vector<double>> der = DerivativeMaker();
			Vector<double>[] res = RungeKutta.FourthOrder(y0, 0, 1000, N, der);

			double[] finalValues = new double[N - iterationsInOneSecond*tStart];

			for (int i = iterationsInOneSecond*tStart; i < N; i++)
			{
				double[] temp = res[i].ToArray();
				finalValues[i - iterationsInOneSecond * tStart] = temp[numberOfCountedMod - 1];
			}

			double[] finalTemp = res[res.Length - 1].ToArray();
			GlobalModes.X = finalTemp[0];
			GlobalModes.Y = finalTemp[1];
			GlobalModes.Z = finalTemp[2];
			GlobalModes.V = finalTemp[3];
			GlobalModes.W = finalTemp[4];

			return finalValues;
		}

		public Func<double, Vector<double>, Vector<double>> DerivativeMaker()
		{
			return (t, B) =>
			{
				double[] A = B.ToArray();
				double X = A[0];
				double Y = A[1];
				double Z = A[2];
				double V = A[3];
				double W = A[4];

				return Vector<double>.Build.Dense(new[] {
					Pr * (-X + r * Y + e * W * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)),
					-Y + X + X * Z,
					-b * Z - X * Y,
					Pr * (-d * V + (r * W - e * Y * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)) / d),
					-d * W + V
			});

			};
		}
	}
}