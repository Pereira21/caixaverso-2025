using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    public class PerfisRiscoController : MainController
    {
        private readonly IPerfilRiscoService _perfilRiscoService;
        
        public PerfisRiscoController(IMapper mapper, INotificador notificador, IPerfilRiscoService perfilRiscoService) : base (mapper, notificador)
        {
            _perfilRiscoService = perfilRiscoService;
        }

        [HttpGet("perfil-risco/{clienteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPerfilRisco(int clienteId)
        {
            var perfilRisco = await _perfilRiscoService.ObterPorClienteId(clienteId);
            return CustomResponse(perfilRisco);
        }

        [HttpGet("produtos-recomendados/{perfil}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterProdutosRecomendadosPorPerfil(string perfil)
        {
            var produtosRecomendados = await _perfilRiscoService.ObterProdutosRecomendadosPorPerfil(perfil);
            return CustomResponse(produtosRecomendados);
        }
    }
}
