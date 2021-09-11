using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SimpleTest.Domain.Services.Math;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SimpleTest.NUnit
{
	/// <summary>
	/// NUnit executa os testes em ordem alfabética, logo é o mais indicado para testes de CRUD e outros tipos de execuções sequenciais
	/// </summary>
	public class CalculatorServiceTest
	{
		private IConfiguration _config;
		private CalculatorService _service;
		private decimal currentValue;

		private readonly Dictionary<string, string> myConfiguration = new Dictionary<string, string>
		{
			{"Operators:Sum", "+"},
			{"Operators:Subtraction", "-"},
			{"Operators:Division", "/"},
			{"Operators:Multiplication", "*"},
		};

		[SetUp]
		public void Setup()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			_config = new ConfigurationBuilder()
			.AddInMemoryCollection(myConfiguration)
			.Build();

			_service = new CalculatorService(_config);
		}

		[Test]
		public void Test1()
		{
			string formula = "2+3";
			decimal expectedResult = 5;

			currentValue = _service.Calculate(formula);

			Assert.AreEqual(expectedResult, currentValue);
		}

		[Test]
		public void Test2()
		{
			string formula = $"{currentValue}-10";
			decimal expectedResult = -5;

			currentValue = _service.Calculate(formula);

			Assert.AreEqual(expectedResult, currentValue);
		}

		[Test]
		public void Test3()
		{
			string formula = $"15/{currentValue}";
			decimal expectedResult = -3;

			currentValue = _service.Calculate(formula);

			Assert.AreEqual(expectedResult, currentValue);
		}

		[Test]
		public void Test4()
		{
			string formula = $"-5    * {currentValue}";
			decimal expectedResult = 15;

			currentValue = _service.Calculate(formula);

			Assert.AreEqual(expectedResult, currentValue);
		}

		[Test]
		public void Test0()
		{
			string formula = "5**15";

			Exception ex = Assert.Throws<Exception>(()=>_service.Calculate(formula));
			Assert.That(ex.Message, Is.EqualTo("Invalid Formula"));
		}
	}
}