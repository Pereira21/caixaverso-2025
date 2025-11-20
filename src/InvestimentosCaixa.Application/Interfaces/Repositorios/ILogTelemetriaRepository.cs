using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ILogTelemetriaRepository : IRepository<LogTelemetria>
    {
        /// <summary>
        /// Obter logs de telemetria mensal paginados
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<TelemetriaResponse>> ObterPaginadoTelemetriaMensalAsync(int pagina, int tamanhoPagina);
    }
}
