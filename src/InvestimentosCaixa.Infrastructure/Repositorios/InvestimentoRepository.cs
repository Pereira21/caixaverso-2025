using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class InvestimentoRepository : Repository<Investimento>, IInvestimentoRepository
    {
        public InvestimentoRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<List<Investimento>> ObterComProdutoPorClienteId(int clienteId)
        {
            return await _dbSet.Include(x => x.Produto).ThenInclude(x => x.TipoProduto).Where(i => i.ClienteId == clienteId).AsNoTracking().ToListAsync();
        }
    }
}
