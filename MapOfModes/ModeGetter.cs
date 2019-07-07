using System;
using System.Collections.Generic;
using System.Linq;


namespace MapOfModes
{
	enum Regime { Chaos, QuasiPeriodic, Fading, SomethingUnknown };
	enum Parameter { Pr, e, r, nu, k };
	enum Mode { X, Y, Z, V, W };

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
			Parameter horizontalParameter, Parameter verticalParameter, Mode mode)
		{
			double Pr = system.Pr;
			double e = system.e;
			double r = system.r;
			double nu = system.nu;
			double k = system.k;

			for (double horizontalValue = horizontalValueStart; horizontalValue < horizontalValueEnd; horizontalValue += horizontalValueStep)
			{
				for (double verticalValue = verticalValueStart; verticalValue < verticalValueEnd; verticalValue += verticalValueStep)
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
					var sys = new ODESystem(Pr, nu, e, r, k, tStart, tEnd, iterationsInOneSecond);

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
	}
}