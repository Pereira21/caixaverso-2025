using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;

namespace InvestimentosCaixa.Application.Services
{
    public class LogTelemetriaService : BaseService, ILogTelemetriaService
    {
        private readonly ILogger<ILogTelemetriaService> _logger;
        private readonly ILogTelemetriaRepository _logTelemetriaRepository;

        public LogTelemetriaService(ILogger<ILogTelemetriaService> logger, INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ILogTelemetriaRepository logTelemetriaRepository) : base(notificador, mapper, unitOfWork)
        {
            _logger = logger;
            _logTelemetriaRepository = logTelemetriaRepository;
        }

        public async Task<List<TelemetriaResponse>> ObterPeriodoMensalAsync(Guid userId, string userEmail)
        {
            _logger.LogInformation($"O admin {userId} - Email: {userEmail} está obtendo os registros de Telemetria Mensal do sistema!");

            return await _logTelemetriaRepository.ObterTelemetriaMensalAsync();
        }
    }
}
