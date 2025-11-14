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
                .Where(p => p.TipoProduto.Nome == tipoProduto && p.PrazoMinimoMeses <= prazoMeses);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
