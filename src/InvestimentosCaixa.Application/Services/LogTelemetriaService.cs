using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;

namespace InvestimentosCaixa.Application.Services
{
    public class LogTelemetriaService : BaseService, ILogTelemetriaService
    {
        private readonly ILogTelemetriaRepository _logTelemetriaRepository;

        public LogTelemetriaService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ILogTelemetriaRepository logTelemetriaRepository) : base(notificador, mapper, unitOfWork)
        {
            _logTelemetriaRepository = logTelemetriaRepository;
        }

        public async Task<List<TelemetriaResponse>> ObterPeriodoMensalAsync()
        {
            return await _logTelemetriaRepository.ObterTelemetriaMensalAsync();
        }
    }
}
