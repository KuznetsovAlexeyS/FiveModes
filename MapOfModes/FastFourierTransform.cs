using System;
using System.Collections.Generic;
using ZedGraph;


namespace MapOfModes
{
	class Tester1
	{
		public static IEnumerable<DataPoint> Test(Random rnd, int leftBorder, int rightBorder, int lowBorder, int highBorder)
		{
			while (true)
			{
				yield return new DataPoint { X = rnd.Next(leftBorder, rightBorder), Y = rnd.Next(lowBorder, highBorder) };
			}
		}
	}
}