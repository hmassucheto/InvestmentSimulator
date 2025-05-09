using InvestmentSimulator.Api.Models;
using InvestmentSimulator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentSimulator.Api.Controllers {
    [ApiController]
    [Route("api/investments")]
    public class InvestmentsController(IInvestmentCalculator investmentCalculator) : ControllerBase {
        private readonly IInvestmentCalculator _investmentCalculator = investmentCalculator;

        [HttpPost]
        [ProducesResponseType(typeof(InvestmentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] InvestmentRequest request) {
            InvestmentResult result = _investmentCalculator.Calculate(request.InitialValue, request.Months);
            return Ok(result);
        }
    }
}
