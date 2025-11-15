using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class TelemetriaController : MainController
    {
        private readonly ILogTelemetriaRepository _telemetriaRepository;

        public TelemetriaController(IMapper mapper, INotificador notificador, ILogTelemetriaRepository telemetriaRepository) : base(mapper, notificador)
        {
            _telemetriaRepository = telemetriaRepository;
        }

        [HttpGet("telemetria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Obter()
        {
            var dados = await _telemetriaRepository.ObterResumoAsync();
            return CustomResponse(dados);
        }
    }
}
