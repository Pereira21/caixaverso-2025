using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class InvestimentoRepository : Repository<Investimento>, IInvestimentoRepository
    {
        public InvestimentoRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<List<Investimento>> ObterComProdutoPorClienteId(int clienteId)
        {
            return await _dbSet.Include(x => x.Produto).ThenInclude(x => x.TipoProduto).Where(i => i.ClienteId == clienteId).AsNoTracking().ToListAsync();
        }
    }
}
