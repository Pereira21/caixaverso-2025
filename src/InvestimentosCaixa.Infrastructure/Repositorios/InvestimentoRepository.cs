using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class InvestimentoRepository : Repository<Investimento>, IInvestimentoRepository
    {
        public InvestimentoRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<List<Investimento>> ObterComProdutoPorClienteIdAsync(int clienteId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Produto)
                    .ThenInclude(x => x.TipoProduto)
                .Where(i => i.ClienteId == clienteId)
                .OrderByDescending(x => x.Data)
                .ToListAsync();
        }

        public async Task<List<Investimento>> ObterPaginadoComProdutoPorClienteIdAsync(int clienteId, int pagina = 1, int tamanhoPagina = 200)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Produto)
                    .ThenInclude(x => x.TipoProduto)
                .Where(i => i.ClienteId == clienteId)
                .OrderByDescending(x => x.Data)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
        }
    }
}
