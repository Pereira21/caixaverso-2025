using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TelemetriaController : MainController
    {
        private readonly ITelemetriaRepository _telemetriaRepository;

        public TelemetriaController(IMapper mapper, INotificador notificador, ITelemetriaRepository telemetriaRepository) : base(mapper, notificador)
        {
            _telemetriaRepository = telemetriaRepository;
        }

        [HttpGet("telemetria")]
        public async Task<IActionResult> Obter()
        {
            var dados = await _telemetriaRepository.ObterResumoAsync();
            return CustomResponse(dados);
        }
    }
}
