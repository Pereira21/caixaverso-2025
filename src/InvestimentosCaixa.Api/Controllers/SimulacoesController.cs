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
    public class SimulacoesController : MainController
    {
        private readonly ISimulacaoService _simulacaoService;
        
        public SimulacoesController(IMapper mapper, INotificador notificador, ISimulacaoService simulacaoService) : base (mapper, notificador)
        {
            _simulacaoService = simulacaoService;
        }

        /// <summary>
        /// Simular investimentos
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Simulação realizada com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        [HttpPost("simular-investimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> SimularInvestimento([FromBody] SimularInvestimentoModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var request = _mapper.Map<SimularInvestimentoRequest>(model);

            var simulacao = await _simulacaoService.SimularInvestimentoAsync(request);

            return CustomResponse(simulacao);
        }

        /// <summary>
        /// Obter simulações realizadas
        /// </summary>
        /// <response code="200">Simulações obtidas com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="403">Permissão insuficiente para acessar este recurso</response>
        [HttpGet("simulacoes")]
        [Authorize(Roles = "analista")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Obter([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 200)
        {
            var historico = await _simulacaoService.ObterHistoricoAsync(UserId.Value, UserEmail, pagina, tamanhoPagina);
            return CustomResponse(historico);
        }

        /// <summary>
        /// Obter simulações realizadas por produto e dia
        /// </summary>
        /// <response code="200">Simulações obtidas com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="403">Permissão insuficiente para acessar este recurso</response>
        [HttpGet("simulacoes/por-produto-dia")]
        [Authorize(Roles = "analista")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterPorProdutoDia([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 200)
        {
            var resultado = await _simulacaoService.ObterPorProdutoDiaAsync(UserId.Value, UserEmail, pagina, tamanhoPagina);
            return Ok(resultado);
        }
    }
}
