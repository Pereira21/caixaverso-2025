using InvestimentosCaixa.Api.Models;
using InvestimentosCaixa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Route("api/[controller]")]
    public class InvestimentosController : MainController
    {
        private readonly ISimulacaoService _simulacaoService;
        
        public InvestimentosController(ISimulacaoService simulacaoService)
        {
            _simulacaoService = simulacaoService;
        }

        [HttpPost("simular-investimento")]
        public async Task<IActionResult> SimularInvestimento([FromBody] SimularInvestimentoModel model)
        {
            //if (!ModelState.IsValid) return CustomResponse(ModelState);

            var simulacao = await _simulacaoService.SimularInvestimento(model.ClienteId, model.Valor, model.PrazoMeses, model.TipoProduto);

            return Ok(simulacao);
            //return CustomResponse(parcelas);
        }
    }
}
