using System;
using System.Collections.Generic;
using System.Linq;


namespace MapOfModes
{
	public enum Regime { Chaos, QuasiPeriodic, Fading, SomethingUnknown };
	public enum Parameter { Pr, e, r, nu, k };
	public enum Mode { X, Y, Z, V, W };

	public class ParserToEnums
	{
		public static Mode ParseToMode(string s)
		{
			Mode parsedOne = new Mode();
			switch (s)
			{
				case "X":
					parsedOne = Mode.X;
					break;
				case "Y":
					parsedOne = Mode.Y;
					break;
				case "Z":
					parsedOne = Mode.Z;
					break;
				case "V":
					parsedOne = Mode.V;
					break;
				case "W":
					parsedOne = Mode.W;
					break;
			}
			return parsedOne;
		}

		public static Parameter ParseToParameter(string s)
		{
			Parameter parsedOne = new Parameter();
			switch (s)
			{
				case "Pr":
					parsedOne = Parameter.Pr;
					break;
				case "e":
					parsedOne = Parameter.e;
					break;
				case "r":
					parsedOne = Parameter.r;
					break;
				case "nu":
					parsedOne = Parameter.nu;
					break;
				case "k":
					parsedOne = Parameter.k;
					break;
			}
			return parsedOne;
		}

		public static Regime ParseToRegime(string s)
		{
			Regime parsedOne = new Regime();
			switch (s)
			{
				case "Chaos":
					parsedOne = Regime.Chaos;
					break;
				case "QuasiPeriodic":
					parsedOne = Regime.QuasiPeriodic;
					break;
				case "Fading":
					parsedOne = Regime.Fading;
					break;
				case "SomethingUnknown":
					parsedOne = Regime.SomethingUnknown;
					break;
			}
			return parsedOne;
		}
	}
}