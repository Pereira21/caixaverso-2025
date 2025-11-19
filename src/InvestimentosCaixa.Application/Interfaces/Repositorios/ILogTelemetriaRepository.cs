using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ILogTelemetriaRepository : IRepository<LogTelemetria>
    {
        Task<List<TelemetriaResponse>> ObterPaginadoTelemetriaMensalAsync(int pagina, int tamanhoPagina);
    }
}
