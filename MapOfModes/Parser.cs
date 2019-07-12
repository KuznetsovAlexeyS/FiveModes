using System;
using System.Linq;

namespace MapOfModes
{
	public class ParserToDouble
	{
		public double Value { get; }
		public bool IsDataCorrect { get; }

		public ParserToDouble(string str) // ����������� ������ � �����. ����� �������� � � �������� "6.13" � � �������� "6,13", 
			//������� �� ������ ������, ��� ������ TryParse
		{
			double k = 0.0;
			char[] number = str.ToCharArray();
			for(int i =0; i<number.Length; i++)
			{
				if (number[i] == '.') number[i] = ',';
			}
			string strMilled = new string(number);

			if (Double.TryParse(strMilled, out k))
			{
				this.Value = k;
				IsDataCorrect = true;
			}
			else
			{
				this.Value = -1;
				IsDataCorrect = false;
			}
		} 
	}
}