using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    /// <summary>
    /// Controlador focado em requisições de telemetria
    /// </summary>
    [Authorize(Roles = "tecnico")]
    public class TelemetriasController : MainController
    {
        private readonly ILogTelemetriaService _logTelemetriaService;

        public TelemetriasController(IMapper mapper, INotificador notificador, ILogTelemetriaService logTelemetriaService) : base(mapper, notificador)
        {
            _logTelemetriaService = logTelemetriaService;
        }


        /// <summary>
        /// Obter Telemetria Mensal
        /// </summary>
        /// <response code="200">Registros de Telemetria retornados com sucesso</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="403">Permissão insuficiente para acessar este recurso</response>
        [HttpGet("telemetria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Obter([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 200)
        {
            var logTelemetria = await _logTelemetriaService.ObterPeriodoMensalAsync(UserId.Value, UserEmail, pagina, tamanhoPagina);

            return CustomResponse(logTelemetria);
        }
    }
}
