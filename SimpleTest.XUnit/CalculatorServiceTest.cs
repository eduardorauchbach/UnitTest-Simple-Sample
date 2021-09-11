using Microsoft.Extensions.Configuration;
using Xunit;
using SimpleTest.Domain.Services.Math;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SimpleTest.XUnit
{
	/// <summary>
	/// XUnit executa os testes sem nenhuma ordem, totalmente aleatórios
	/// </summary>
	public class CalculatorServiceTest
	{
		private IConfiguration _config;
		private CalculatorService _service;

		private readonly Dictionary<string, string> myConfiguration = new Dictionary<string, string>
		{
			{"Operators:Sum", "+"},
			{"Operators:Subtraction", "-"},
			{"Operators:Division", "/"},
			{"Operators:Multiplication", "*"},
		};

		public CalculatorServiceTest()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			_config = new ConfigurationBuilder()
			.AddInMemoryCollection(myConfiguration)
			.Build();

			_service = new CalculatorService(_config);
		}

		[Fact]
		public void TestSum()
		{
			string formula = "2+3";
			decimal expectedResult = 5;

			decimal result = _service.Calculate(formula);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void TestSubtraction()
		{
			string formula = "5-10";
			decimal expectedResult = -5;

			decimal result = _service.Calculate(formula);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void TestDivision()
		{
			string formula = "15/5";
			decimal expectedResult = 3;

			decimal result = _service.Calculate(formula);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void TestMultiplication()
		{
			string formula = "5    * 5";
			decimal expectedResult = 25;

			decimal result = _service.Calculate(formula);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void TestFormatFail()
		{
			string formula = "5**15";

			Exception ex = Assert.Throws<Exception>(()=>_service.Calculate(formula));
			Assert.Equal("Invalid Formula", ex.Message);
		}
	}
}