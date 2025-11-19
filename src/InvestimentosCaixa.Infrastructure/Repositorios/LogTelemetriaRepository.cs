using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class LogTelemetriaRepository : Repository<LogTelemetria>, ILogTelemetriaRepository
    {
        public LogTelemetriaRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<List<TelemetriaResponse>> ObterPaginadoTelemetriaMensalAsync(int pagina, int tamanhoPagina)
        {
            var registros = await _dbSet
                .GroupBy(r => new
                {
                    r.Endpoint,
                    Ano = r.DataRegistro.Year,
                    Mes = r.DataRegistro.Month
                })
                .Select(g => new
                {
                    Nome = g.Key.Endpoint,
                    Ano = g.Key.Ano,
                    Mes = g.Key.Mes,
                    QuantidadeChamadas = g.Count(),
                    MediaTempoRespostaMs = Math.Round(g.Average(x => x.TempoRespostaMs), 2)
                })
                .ToListAsync();

            // trnasformando resultado em mês
            var resposta = registros
                .GroupBy(r => new { r.Ano, r.Mes })
                .Select(g => new TelemetriaResponse
                {
                    Servicos = g.Select(x => new ServicoResponse
                    {
                        Nome = x.Nome,
                        QuantidadeChamadas = x.QuantidadeChamadas,
                        MediaTempoRespostaMs = x.MediaTempoRespostaMs
                    }).ToList(),
                    Periodo = new PeriodoResponse
                    {
                        Inicio = new DateOnly(g.Key.Ano, g.Key.Mes, 1),
                        Fim = new DateOnly(g.Key.Ano, g.Key.Mes,
                            DateTime.DaysInMonth(g.Key.Ano, g.Key.Mes))
                    }
                })
                .OrderByDescending(x => x.Periodo.Inicio)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            return resposta;
        }

    }
}
