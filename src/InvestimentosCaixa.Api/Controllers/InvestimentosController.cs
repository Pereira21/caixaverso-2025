using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize(Roles = "analista")]
    public class InvestimentosController : MainController
    {
        private readonly IInvestimentoService _investimentoService;
        
        public InvestimentosController(IMapper mapper, INotificador notificador, IInvestimentoService investimentoService) : base (mapper, notificador)
        {
            _investimentoService = investimentoService;
        }

        /// <summary>
        /// Obter Investimentos por ClienteId
        /// </summary>
        /// <param name="clienteId"></param>
        /// <response code="200">Investimentos retornados com sucesso</response>
        /// <response code="200">Não há investimentos para o Cliente solicitado</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="403">Permissão insuficiente para acessar este recurso</response>
        /// <response code="404">O registro solicitado não existe na base de dados</response>
        [HttpGet("investimentos/{clienteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorClienteId(int clienteId, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 200)
        {
            var investimentos = await _investimentoService.ObterPorClienteIdAsync(UserId.Value, UserEmail, clienteId, pagina, tamanhoPagina);

            if(investimentos == null || !investimentos.Any())
                return NoContent();

            return CustomResponse(investimentos);
        }
    }
}
