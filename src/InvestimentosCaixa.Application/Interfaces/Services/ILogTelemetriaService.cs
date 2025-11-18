
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ILogTelemetriaService
    {
        Task<List<TelemetriaResponse>> ObterPeriodoMensalAsync();
    }
}
