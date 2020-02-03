using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfModes
{
	class RungeKuttaSolver
	{
		private double[][] k; // для каждой компоненты вектора будет запасено 5 k
		public double[][] Solution { private set; get; } // первый элемент - шаг, все последующие - соответствующие компоненты верктора
		private double step;
		private double currentStep;
		private double currentValue;
		private int amountOfSteps;
		private int amountOfEquations;
		private Func<double[], double, double[]> functions;
		public readonly double maxError;

		public double MaxOccuredError { private set; get; }

		public RungeKuttaSolver(double[] startValues, double startValue, int amountOfSteps, double step, double maxPossibleError, Func<double[], double, double[]> functions)
		{
			this.functions = functions;

			this.amountOfEquations = startValues.Count();
			k = new double[amountOfEquations][];
			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i] = new double[5];
			}

			Solution = new double[amountOfSteps + 1][];
			for (int i = 0; i < amountOfSteps + 1; i++)
			{
				Solution[i] = new double[amountOfEquations+1];
			}
			Solution[0][0] = startValue;

			for (int i = 1; i < amountOfEquations+1; i++)
			{
				Solution[0][i] = startValues[i-1];
			}
			currentValue = startValue; // Для понятия этого минуса см. реализацию изменения currentStep в методе Solve

			this.amountOfSteps = amountOfSteps + 1;
			this.step = step;
			this.currentStep = step / 10;
			this.maxError = maxPossibleError;

			MaxOccuredError = 0;
		}

		public double[][] Solve()
		{
			for (int i = 0; i < amountOfSteps - 1; i++)
			{
				CountWithDifferentStep(i, currentValue, currentValue + step);
				currentValue += step;
				Solution[i + 1][0] = currentValue;
				double error = CountError();
				if (MaxOccuredError < error)
				{
					MaxOccuredError = error;
				}
			}

			return Solution;
		}

		private void CountWithDifferentStep(int numberOfCountedValues, double startValue, double finalValue)
		{
			double[] currentValues = new double[amountOfEquations];

			for (int i = 0; i < amountOfEquations; i++)
			{
				currentValues[i] = Solution[numberOfCountedValues][i+1];
			}

			double currentValue = startValue;

			while (true)
			{
				if (currentValue + currentStep < finalValue) // Удостоверимся, что значение не превосходит финальное значение
				{
					CountNextValues(currentValues, currentValue, currentStep); // Перезапишем коэфы k1 ... k5
					double error = CountError();
					if (error > maxError) // Уменьшаем шаг, если ошибка большая, и перейдём в начало цикла
					{
						currentStep /= 2.0;
						continue;
					}
					else // Если ошибка небольшая, то перезаписываем значения
					{
						FillCountedValues(currentValues);
						currentValue += currentStep;

						if (MaxOccuredError < error)
						{
							MaxOccuredError = error;
						}

						if (128 * error < maxError) currentStep *= 2.0; // Если ошибка маленькая, то увеличиваем шаг
					}
				}
				else // Если сумма текущего значения и шага больше ожидаемого финального значения
				{
					CountNextValues(currentValues, currentValue, finalValue - currentValue); // Расчитаем значение в той точке, в которой будет запись
					if (CountError() < maxError) // Если нас устраивает ошибка
					{
						FillCountedValues(currentValues); // То перезапишем значения и выйдем из цикла
						break;
					}
					else
					{
						currentStep = currentStep / 3.0; // Иначе уменьшим шаг (в этом случае условие из начала цикла может начать вновь выполняться)
					}
				}
			}

			for(int i=1; i<amountOfEquations+1; i++)
			{
				Solution[numberOfCountedValues + 1][i] = currentValues[i - 1];
			}
		}

		private double CountError()
		{
			double tempMaxError = 0.0;

			for (int i = 0; i < amountOfEquations; i++)
			{
				var possibleMaxError = ((2 * k[i][0]) - (9 * k[i][2]) + (8 * k[i][3]) - k[i][4]) / 30.0;

				if (tempMaxError < possibleMaxError)
				{
					tempMaxError = possibleMaxError;
				}
			}

			return Math.Abs(tempMaxError);
		}

		private void CountNextValues(double[] baseEquations, double currentValue, double currentStep) // Сокращённая запись последовательного вызова перерасчёта k-шек
		{
			CountK1(baseEquations, currentValue, currentStep);
			CountK2(baseEquations, currentValue, currentStep);
			CountK3(baseEquations, currentValue, currentStep);
			CountK4(baseEquations, currentValue, currentStep);
			CountK5(baseEquations, currentValue, currentStep);
		}

		private void FillCountedValues(double[] baseArray) // Перезапись значений
		{
			for (int i = 0; i < amountOfEquations; i++)
			{
				baseArray[i] = baseArray[i] +
					(k[i][0] / 6.0) +
					(4.0 / 6.0 * k[i][3]) +
					(k[i][4] / 6.0);
			}
		}

		/*public void Save(int numberOfSavedEquation)
		{
			var savedStrings = new string[amountOfSteps];
			for (int i = 0; i < amountOfSteps; i++)
			{
				savedStrings[i] = (i * step).ToString() + " " + Solution[i][numberOfSavedEquation].ToString();
			}
			File.WriteAllLines("SavedData" + numberOfSavedEquation.ToString() + ".txt", savedStrings);
		}*/

		//Счёт k-шек
		#region
		private void CountK1(double[] countedArray, double currentT, double currentDeltaT) // Начало расчёта k-шек
		{
			if (countedArray.Count() != amountOfEquations) throw new ArgumentException("Количество элементов переданного массива не соответствует количеству уравнений!");

			double[] K1Array;
			K1Array = functions(countedArray, currentT);

			K1Array = K1Array
				.Select(n => n * currentDeltaT)
				.ToArray();

			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i][0] = K1Array[i];
			}
		}

		private void CountK2(double[] countedArray, double currentT, double currentDeltaT)
		{
			if (countedArray.Count() != amountOfEquations) throw new ArgumentException("Количество элементов переданного массива не соответствует количеству уравнений!");

			double[] K2Array;

			double[] vectorSumForK2 = new double[amountOfEquations];
			for (int i = 0; i < amountOfEquations; i++)
			{
				vectorSumForK2[i] = countedArray[i]
					+ (k[i][0] / 3.0);
			}

			K2Array = functions(vectorSumForK2, currentT + (currentDeltaT / 3.0));
			K2Array = K2Array
				.Select(n => n * currentDeltaT)
				.ToArray();

			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i][1] = K2Array[i];
			}
		}

		private void CountK3(double[] countedArray, double currentT, double currentDeltaT)
		{
			if (countedArray.Count() != amountOfEquations) throw new ArgumentException("Количество элементов переданного массива не соответствует количеству уравнений!");

			double[] K3Array;

			double[] vectorSumForK3 = new double[amountOfEquations];
			for (int i = 0; i < amountOfEquations; i++)
			{
				vectorSumForK3[i] = countedArray[i]
					+ (k[i][0] / 6.0)
					+ (k[i][1] / 6.0);
			}

			K3Array = functions(vectorSumForK3, currentT + (currentDeltaT / 3.0));
			K3Array = K3Array
				.Select(n => n * currentDeltaT)
				.ToArray();

			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i][2] = K3Array[i];
			}
		}

		private void CountK4(double[] countedArray, double currentT, double currentDeltaT)
		{
			if (countedArray.Count() != amountOfEquations) throw new ArgumentException("Количество элементов переданного массива не соответствует количеству уравнений!");

			double[] K4Array;

			double[] vectorSumForK4 = new double[amountOfEquations];
			for (int i = 0; i < amountOfEquations; i++)
			{
				vectorSumForK4[i] = countedArray[i]
					+ (k[i][0] / 8.0)
					+ (k[i][2] * 3.0 / 8.0);
			}

			K4Array = functions(vectorSumForK4, currentT + (currentDeltaT / 2.0));
			K4Array = K4Array
				.Select(n => n * currentDeltaT)
				.ToArray();

			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i][3] = K4Array[i];
			}
		}

		private void CountK5(double[] countedArray, double currentT, double currentDeltaT)
		{
			if (countedArray.Count() != amountOfEquations) throw new ArgumentException("Количество элементов переданного массива не соответствует количеству уравнений!");

			double[] K5Array;

			double[] vectorSumForK5 = new double[amountOfEquations];
			for (int i = 0; i < amountOfEquations; i++)
			{
				vectorSumForK5[i] = countedArray[i]
					+ (k[i][0] / 2.0)
					- (k[i][2] * 1.5)
					+ (k[i][3] * 2);
			}

			K5Array = functions(vectorSumForK5, currentT + currentDeltaT);
			K5Array = K5Array
				.Select(n => n * currentDeltaT)
				.ToArray();

			for (int i = 0; i < amountOfEquations; i++)
			{
				k[i][4] = K5Array[i];
			}
		} // Конец расчёта k-шек
		#endregion
	}
}
