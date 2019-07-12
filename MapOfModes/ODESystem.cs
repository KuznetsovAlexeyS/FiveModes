using System;
using DotNumerics.ODE;

namespace MapOfModes
{
	public partial class ODESystem
	{
		private OdeExplicitRungeKutta45 odeRK = new OdeExplicitRungeKutta45();

		public double Pr { get; } // ����� ��������
		public double nu { get; } // ������� �������������� ����
		public double e { get; } // ������������� ����� �����
		public double r { get; } // �������� ����� �����
		public double k { get; } // �������� �����
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
			if (tStart >= tEnd || tEnd > 1000) throw new ArgumentException(); // ��������� ����� �� ����� ���� ������ ��������� �������, 
			//�����, �� ����������� ������������, �������� ����� �� ����� ���� ������ 1000. ���� ������ ������ -- ����������� ������ � ���� 
			//� ����������� ������� ��� �������� ��������� ��������.
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
			OdeFunction fun = new OdeFunction(ODEs);
			double[] y0 = new double[5];
			y0[0] = startX; // ��������� �������
			y0[1] = startY;
			y0[2] = startZ;
			y0[3] = startV;
			y0[4] = startW;
			this.odeRK.InitializeODEs(fun, 5);
			double[,] sol = odeRK.Solve(y0, 0, 1.0/iterationsInOneSecond, tEnd); // iterationsInOneSeconds -- ��������, �������� ����.
			// tEnd -- �����, �� �������� �������.
			double[] mod = new double [sol.GetLength(0) - tStart * iterationsInOneSecond]; // �������� �� �� ������� ������� tStart.
			int amountOfPoints = sol.GetLength(0);
			for(int i=tStart * iterationsInOneSecond; i < amountOfPoints; i++)
			{
				mod[i - tStart*iterationsInOneSecond] = sol[i, numberOfCountedMod];
			}
			GlobalModes.X = sol[amountOfPoints - 1, 1]; // �� ������ ����������� �� ���������. 
			GlobalModes.Y = sol[amountOfPoints - 1, 2]; // ����� ��������������, ������ 5 ���������� �� ������ ��������, 
			GlobalModes.Z = sol[amountOfPoints - 1, 3]; // ���� ��������� ��������� ���������� ����������, ������� ���������� ���������� � ODESystem
			GlobalModes.V = sol[amountOfPoints - 1, 4];
			GlobalModes.W = sol[amountOfPoints - 1, 5];
			return mod;
		}

		double[] yprime = new double[5];

		private double[] ODEs(double t, double[] y)
		{
			yprime[0] = Pr * (-y[0] + r * y[1] + e * y[4] * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)); // ������� ���������
			yprime[1] = -y[1] + y[0] + y[0] * y[2];
			yprime[2] = -b * y[2] - y[0] * y[1];
			yprime[3] = Pr * (-d * y[3] + (r * y[4] - e * y[1] * Math.Cos(2 * Math.PI * nu * t) * Math.Cos(2 * Math.PI * nu * t)) / d);
			yprime[4] = -d * y[4] + y[3];
			return this.yprime;
		}
	}
}