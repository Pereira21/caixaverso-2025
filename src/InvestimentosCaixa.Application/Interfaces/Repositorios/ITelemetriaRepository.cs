using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ITelemetriaRepository : IRepository<Telemetria>
    {
        Task<IEnumerable<dynamic>> ObterResumoAsync();
    }
}
