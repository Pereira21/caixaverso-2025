using AutoMapper;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            _logger.LogInformation($"O técnico {userId} - Email: {userEmail} está obtendo os registros de Telemetria Mensal do sistema!");

            return await _logTelemetriaRepository.ObterTelemetriaMensalAsync();
        }

        public async Task AdicionarAsync(LogTelemetriaRequest request)
        {
            var registro = new LogTelemetria(
                endpoint: request.Endpoint,
                metodo: request.Metodo,
                tempoRespostaMs: request.TempoRespostaMs,
                sucesso: request.Sucesso,
                dataRegistro: request.DataRegistro
            );

            await _logTelemetriaRepository.AdicionarAsync(registro);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
