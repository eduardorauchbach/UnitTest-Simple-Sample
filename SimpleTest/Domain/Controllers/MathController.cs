using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SimpleTest.Domain.Services.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleTest.Domain.Controllers
{
	[EnableCors(Startup.DefaultPolityName)]
	[Route("api/[controller]")]
	[ApiController]
	public class MathController : ControllerBase
	{
        private readonly CalculatorService _calculatorService;

        public MathController(CalculatorService calculatorService)
		{
            _calculatorService = calculatorService;

        }

        /// <summary>
        /// Calculos simples efetuados em ordem como em uma calculadora simples
        /// </summary>
        /// <param name="formula">2 + 2 / 4 x 2</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string formula)
        {
            ObjectResult response;
            decimal result;

            try
            {
                result = _calculatorService.Calculate(formula);
                response = Ok(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(500, ex.Message);
            }

            return response;
        }
    }
}
