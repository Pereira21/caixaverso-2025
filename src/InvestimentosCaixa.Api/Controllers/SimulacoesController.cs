using AutoMapper;
using InvestimentosCaixa.Api.Models.Simulacao;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SimulacoesController : MainController
    {
        private readonly ISimulacaoService _simulacaoService;
        
        public SimulacoesController(IMapper mapper, INotificador notificador, ISimulacaoService simulacaoService) : base (mapper, notificador)
        {
            _simulacaoService = simulacaoService;
        }

        [HttpPost("simular-investimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> SimularInvestimento([FromBody] SimularInvestimentoModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var request = _mapper.Map<SimularInvestimentoRequest>(model);

            var simulacao = await _simulacaoService.SimularInvestimento(request);

            return CustomResponse(simulacao);
        }

        [HttpGet("simulacoes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Obter()
        {
            var historico = await _simulacaoService.ObterHistorico();
            return CustomResponse(historico);
        }

        [HttpGet("simulacoes/por-produto-dia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ObterPorProdutoDia()
        {
            var resultado = await _simulacaoService.ObterPorProdutoDiaAsync();
            return Ok(resultado);
        }
    }
}
