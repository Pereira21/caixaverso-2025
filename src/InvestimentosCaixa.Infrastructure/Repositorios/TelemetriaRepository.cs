using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class TelemetriaRepository : Repository<Telemetria>, ITelemetriaRepository
    {
        public TelemetriaRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<IEnumerable<dynamic>> ObterResumoAsync()
        {
            return await _context.Set<Telemetria>()
                .GroupBy(t => new { t.Endpoint, t.Metodo })
                .Select(g => new
                {
                    Endpoint = g.Key.Endpoint,
                    Metodo = g.Key.Metodo,
                    TotalRequisicoes = g.Count(),
                    Sucesso = g.Count(x => x.Sucesso),
                    Erros = g.Count(x => !x.Sucesso),
                    TempoMedioMs = g.Average(x => x.TempoRespostaMs),
                    UltimaRequisicao = g.Max(x => x.DataRegistro)
                })
                .ToListAsync();
        }
    }
}
