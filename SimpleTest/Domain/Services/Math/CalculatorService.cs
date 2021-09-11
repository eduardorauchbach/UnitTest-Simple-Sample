using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleTest.Domain.Services.Math
{
	public class CalculatorService
	{
		private readonly IConfiguration _configuration;

		//Também um pouco desnecessário, mas para o intuito do exemplo é válido
		private readonly string _sum;
		private readonly string _subtraction;
		private readonly string _division;
		private readonly string _multiplication;

		public CalculatorService(IConfiguration configuration)
		{
			_configuration = configuration;

			_sum = _configuration.GetValue<string>("Operators:Sum");
			_subtraction = _configuration.GetValue<string>("Operators:Subtraction");
			_division = _configuration.GetValue<string>("Operators:Division");
			_multiplication = _configuration.GetValue<string>("Operators:Multiplication");
		}

		/// <summary>
		/// Função simples de cálculo (poderia ser estática, mas para o intuito do exemplo está normal)
		/// </summary>
		/// <param name="formula">1 + 2 / 3 * 4</param>
		/// <returns></returns>
		public decimal Calculate(string formula)
		{
			decimal currentCalc = 0;

			string[] pieces = null;

			try
			{
				pieces = GetPieces(formula);
				if (pieces != null)
				{
					for (int i = 0; i < pieces.Length; i++)
					{
						if (i % 2 == 0)
						{
							if (decimal.TryParse(pieces[i], out var output))
							{
								if (i != 0)
								{
									if(pieces[i - 1] == _sum)
									{
										currentCalc = Sum(currentCalc, output); break;
									}
									else if (pieces[i - 1] == _subtraction)
									{
										currentCalc = Subtract(currentCalc, output); break;
									}
									else if (pieces[i - 1] == _division)
									{
										currentCalc = Divide(currentCalc, output); break;
									}
									else if (pieces[i - 1] == _multiplication)
									{
										currentCalc = Multiply(currentCalc, output); break;
									}
								}
								else
								{
									currentCalc = output;
								}
							}
							else
							{
								throw new Exception("Inválid Decimal: " + pieces[i]);
							}
						}
					}
				}
				else
				{
					throw new Exception("Invalid Formula");
				}
			}
			catch (Exception ex)
			{
				throw (ex);
			}

			return currentCalc;
		}

		private static decimal Sum(decimal a, decimal b)
		{
			return a + b;
		}

		private static decimal Subtract(decimal a, decimal b)
		{
			return a - b;
		}

		private static decimal Divide(decimal a, decimal b)
		{
			return a / b;
		}

		private static decimal Multiply(decimal a, decimal b)
		{
			return a * b;
		}

		private string[] GetPieces(string formula)
		{
			string[] pieces = null;
			bool lastNumber = false;
			bool onlyNumber = false;

			formula = formula.Replace(" ", "").ToLower();
			if (Regex.IsMatch(formula, $@"^[\-\d\.]+([\{_sum}\{_subtraction}\{_multiplication}\{_division}][\-\d\.]+)*$"))
			{
				pieces = new string[] { };

				foreach (char c in formula)
				{
					var last = pieces.Length - 1;

					if (Char.IsNumber(c) || c == '.')
					{
						if (lastNumber || onlyNumber)
						{
							pieces[last] = pieces[last] + c;
						}
						else
						{
							pieces = pieces.Append(c.ToString()).ToArray();
						}
						lastNumber = true;
						onlyNumber = false;
					}
					else if (c == '-')
					{
						if (onlyNumber)
						{
							pieces = null;
							break;
						}
						else if (!lastNumber)
						{
							onlyNumber = true;
						}

						pieces = pieces.Append(c.ToString()).ToArray();
						lastNumber = false;
					}
					else
					{
						if (onlyNumber)
						{
							pieces = null;
							break;
						}
						pieces = pieces.Append(c.ToString()).ToArray();
						lastNumber = false;
					}
				}
			}

			return pieces;
		}
	}
}
