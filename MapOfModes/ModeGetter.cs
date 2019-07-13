using System;
using System.Collections.Generic;
using System.Linq;


namespace MapOfModes
{
	class ModeInSpace
	{
		public Regime Regime { get; }
		public double HorizontalAxis { get; }
		public double VerticalAxis { get; }

		public ModeInSpace(Regime regime, double horizontalAxis, double verticalAxis)
		{
			this.Regime = regime;
			this.HorizontalAxis = horizontalAxis;
			this.VerticalAxis = verticalAxis;
		}
	}

	class ModeGetter
	{
		public static IEnumerable<ModeInSpace> GoThroughValuesAndGetMode(ODESystem system,
			double horizontalValueStart, double horizontalValueStep, double horizontalValueEnd,
			double verticalValueStart, double verticalValueStep, double verticalValueEnd,
			int tStart, int tEnd, int iterationsInOneSecond,
			double startX, double startY, double startZ, double startV, double startW, bool CBMV,
			Parameter horizontalParameter, Parameter verticalParameter, Mode mode)
		{
			double Pr = system.Pr;
			double e = system.e;
			double r = system.r;
			double nu = system.nu;
			double k = system.k;

			GlobalModes.X = startX; // Если считаем с продолжением по параметру, то будут использоваться GlobalModes, 
			GlobalModes.Y = startY; // которым присваивается последнее значение мод, подробнее см. метод Solve классе ODESystem
			GlobalModes.Z = startZ;
			GlobalModes.V = startV;
			GlobalModes.W = startW;

			var X = startX;
			var Y = startY;
			var Z = startZ;
			var V = startV;
			var W = startW;

			for (double horizontalValue = horizontalValueStart; 
				MakingModesUntil(horizontalValueStart, horizontalValue, horizontalValueEnd); 
				horizontalValue = MakeStep(horizontalValueStart, horizontalValue, horizontalValueStep, horizontalValueEnd))
			{

				switch (horizontalParameter)
				{
					case Parameter.Pr:
						Pr = horizontalValue;
						break;
					case Parameter.r:
						r = horizontalValue;
						break;
					case Parameter.e:
						e = horizontalValue;
						break;
					case Parameter.nu:
						nu = horizontalValue;
						break;
					case Parameter.k:
						k = horizontalValue;
						break;
				}

				for (double verticalValue = verticalValueStart;
				MakingModesUntil(verticalValueStart, verticalValue, verticalValueEnd);
				verticalValue = MakeStep(verticalValueStart, verticalValue, verticalValueStep, verticalValueEnd))
				{
					switch (verticalParameter)
					{
						case Parameter.Pr:
							Pr = verticalValue;
							break;
						case Parameter.r:
							r = verticalValue;
							break;
						case Parameter.e:
							e = verticalValue;
							break;
						case Parameter.nu:
							nu = verticalValue;
							break;
						case Parameter.k:
							k = verticalValue;
							break;
					}

					if (CBMV)
					{
						X = GlobalModes.X;
						Y = GlobalModes.Y;
						Z = GlobalModes.Z;
						V = GlobalModes.V;
						W = GlobalModes.W;
					}
					var sys = new ODESystem(Pr, nu, e, r, k,
							X, Y, Z, V, W,
							tStart, tEnd, iterationsInOneSecond);

					switch (mode) // Аргумент в Solve для моды X -- 1, Y -- 2, Z -- 3, V -- 4, W -- 5. 
					{
						case Mode.X:
							yield return new ModeInSpace(GetMode(sys.Solve(1)), horizontalValue, verticalValue);
							break;
						case Mode.Y:
							yield return new ModeInSpace(GetMode(sys.Solve(2)), horizontalValue, verticalValue);
							break;
						case Mode.Z:
							yield return new ModeInSpace(GetMode(sys.Solve(3)), horizontalValue, verticalValue);
							break;
						case Mode.V:
							yield return new ModeInSpace(GetMode(sys.Solve(4)), horizontalValue, verticalValue);
							break;
						case Mode.W:
							yield return new ModeInSpace(GetMode(sys.Solve(5)), horizontalValue, verticalValue);
							break;
					}
				}
			}
		}

		public static Regime GetMode(double[] origFunction)
		{
			if (origFunction.Max() < 0.1) return Regime.Fading;
			double[] funcAfterFFT = FFT.GetFFT(origFunction).Take(3000).ToArray();
			int extremumCounter = 0;
			for (int i = 2; i < funcAfterFFT.Length; i++)
			{
				if ((funcAfterFFT[i - 2] > funcAfterFFT[i - 1] && funcAfterFFT[i] > funcAfterFFT[i - 1])
					|| (funcAfterFFT[i - 2] < funcAfterFFT[i - 1] && funcAfterFFT[i] < funcAfterFFT[i - 1])) extremumCounter++;
			} // Добавить медианное среднеквадратичное отклонение вместо числа экстремумов для определения хаоса и определение квазипериодических зон.
			if (extremumCounter * 3 > funcAfterFFT.Length) return Regime.Chaos;
			return Regime.QuasiPeriodic;
		}

		private static bool MakingModesUntil(double startValue, double currentValue, double finalValue)// На тот случай, если мы захотим идти от большего к меньшему
		{
			double eps = 0.000000001;
			if (startValue > finalValue && currentValue > finalValue - eps) return true;
			if (startValue > finalValue && currentValue < finalValue) return false;
			if (startValue < finalValue && currentValue < finalValue + eps) return true;
			return false;
		}

		private static double MakeStep(double startValue, double currentValue, double step, double finalValue)
		{
			if (startValue > finalValue) return currentValue - step;
			return currentValue + step;
		}
	}
}