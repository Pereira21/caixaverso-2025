using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PerfilRiscoController : MainController
    {
        private readonly IPerfilRiscoService _perfilRiscoService;
        
        public PerfilRiscoController(IMapper mapper, INotificador notificador, IPerfilRiscoService perfilRiscoService) : base (mapper, notificador)
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
    }
}
