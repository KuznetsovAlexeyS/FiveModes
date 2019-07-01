using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;
using MathNet.Numerics;

namespace MapOfModes
{
	class FFT
	{
		public static double[] GetFFT(double[] originalFunction)
		{
			int nPoints = 524288; // 2^19. nPoints желательно быть степенью двойки для ускорения работы FFT.

			double[] origCut = originalFunction
				.Take(nPoints)
				.ToArray();

			Complex[] resultComplex = origCut
				.Select(x => new Complex(x, 0.0))
				.ToArray();

			Fourier.Forward(resultComplex, FourierOptions.Matlab);

			double[] result = resultComplex
				.Select(x => Math.Abs(x.Real))
				.ToArray();

			return result;
		}
	}
}