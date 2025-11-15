using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ILogTelemetriaRepository : IRepository<LogTelemetria>
    {
        Task<IEnumerable<dynamic>> ObterResumoAsync();
    }
}
