using System;
using System.Collections.Generic;
using System.Linq;


namespace MapOfModes
{
	enum Mode { Chaos, QuasiPeriodic, Fading, SomethingUnknown };

	class ModeGetter
	{
		public static Mode GetMode(double[] origFunction)
		{
			if (origFunction.Max() < 0.1) return Mode.Fading;
			double[] funcAfterFFT = FFT.GetFFT(origFunction).Take(3000).ToArray();
			int extremumCounter = 0;
			for (int i = 2; i < funcAfterFFT.Length; i++)
			{
				if ((funcAfterFFT[i - 2] > funcAfterFFT[i - 1] && funcAfterFFT[i] > funcAfterFFT[i - 1])
					|| (funcAfterFFT[i - 2] < funcAfterFFT[i - 1] && funcAfterFFT[i] < funcAfterFFT[i - 1])) extremumCounter++;
			}
			if (extremumCounter * 3 > funcAfterFFT.Length) return Mode.Chaos;
			return Mode.QuasiPeriodic;
		}
	}
}