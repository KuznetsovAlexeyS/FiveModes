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
			/*
			 * 				var step = 0;
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
			*/
			#endregion //  // Для получения картин FFT и зависимости моды от времени см. сюда
		}
	}
}