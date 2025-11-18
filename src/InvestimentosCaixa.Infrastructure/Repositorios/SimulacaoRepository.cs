using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class SimulacaoRepository : Repository<Simulacao>, ISimulacaoRepository
    {
        public SimulacaoRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<List<Simulacao>> ObterTodosComProdutoAsync()
        {
            return await _dbSet.Include(x => x.Produto).ToListAsync();
        }

        public async Task<List<Simulacao>> ObterComProdutoPorClienteId(int clienteId)
        {
            return await _dbSet
                .Include(x => x.Produto)
                    .ThenInclude(x => x.TipoProduto)
                .Where(x => x.ClienteId == clienteId).ToListAsync();
        }

        public async Task<List<SimulacaoPorProdutoDiaResponse>> ObterSimulacoesPorProdutoDiaAsync()
        {
            return await _dbSet
                .Include(s => s.Produto)
                .GroupBy(s => new
                {
                    s.Produto.Nome,
                    Dia = DateOnly.FromDateTime(s.DataSimulacao)
                })
                .Select(g => new SimulacaoPorProdutoDiaResponse
                {
                    Produto = g.Key.Nome,
                    Data = g.Key.Dia,
                    QuantidadeSimulacoes = g.Count(),
                    MediaValorFinal = Math.Round(g.Average(x => x.ValorInvestido), 2)
                })
                .OrderByDescending(x => x.Data)
                    .ThenBy(x => x.Produto)
                .ToListAsync();
        }
    }
}
