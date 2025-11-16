using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<Produto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(p => p.TipoProduto)
                    .ThenInclude(p => p.Risco)
                .Where(p => p.TipoProduto.Nome == tipoProduto && p.PrazoMinimoMeses <= prazoMeses);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Produto>> ObterPorRiscoAsync(List<int> riscoIdList)
        {
            return await _dbSet
                .Include(x => x.TipoProduto)
                    .ThenInclude(x => x.Risco)
                .Where(p => riscoIdList.Contains(p.TipoProduto.RiscoId))
                .ToListAsync();
        }
    }
}
