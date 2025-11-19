
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ILogTelemetriaService
    {
        Task AdicionarAsync(LogTelemetriaRequest request);
        Task<List<TelemetriaResponse>> ObterPeriodoMensalAsync(Guid userId, string userEmail, int pagina, int tamanhoPagina);
    }
}
