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

        [HttpGet("investimentos/{clienteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorClienteId(int clienteId)
        {
            var investimentos = await _investimentoService.ObterPorClienteId(clienteId);

            if(investimentos == null || !investimentos.Any())
                return NoContent();

            return CustomResponse(investimentos);
        }
    }
}
