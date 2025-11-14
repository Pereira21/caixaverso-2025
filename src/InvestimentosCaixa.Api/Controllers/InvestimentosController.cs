using AutoMapper;
using InvestimentosCaixa.Api.Models;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Route("api/[controller]")]
    public class InvestimentosController : MainController
    {
        private readonly ISimulacaoService _simulacaoService;
        
        public InvestimentosController(IMapper mapper, ISimulacaoService simulacaoService) : base (mapper)
        {
            _simulacaoService = simulacaoService;
        }

        [HttpPost("simular-investimento")]
        public async Task<IActionResult> SimularInvestimento([FromBody] SimularInvestimentoModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var request = _mapper.Map<SimularInvestimentoRequestDTO>(model);

            var simulacao = await _simulacaoService.SimularInvestimento(request);

            return Ok(simulacao);
        }

        [HttpGet("simulacoes")]
        public async Task<IActionResult> Get()
        {
            var historico = await _simulacaoService.ObterHistorico();
            return Ok(historico);
        }
    }
}
