using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize(Roles = "admin")]
    public class TelemetriasController : MainController
    {
        private readonly ILogTelemetriaService _logTelemetriaService;

        public TelemetriasController(IMapper mapper, INotificador notificador, ILogTelemetriaService logTelemetriaService) : base(mapper, notificador)
        {
            _logTelemetriaService = logTelemetriaService;
        }

        [HttpGet("telemetria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Obter()
        {
            var logTelemetria = await _logTelemetriaService.ObterPeriodoMensalAsync();

            return CustomResponse(logTelemetria);
        }
    }
}
