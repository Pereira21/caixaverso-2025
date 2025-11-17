using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const string ProdutosCache = "ProdutosCache";
        public ProdutoRepository(InvestimentosCaixaDbContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Produto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto)
        {
            var produtos = await _memoryCache.GetOrCreateAsync(ProdutosCache, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return await ObterProdutosComTipoAsync();
            });

            return produtos
                .Where(p => p.TipoProduto.Nome == tipoProduto && p.PrazoMinimoMeses <= prazoMeses)
                .FirstOrDefault();
        }

        public async Task<List<Produto>> ObterPorRiscoAsync(List<int> riscoIdList)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.TipoProduto)
                    .ThenInclude(x => x.Risco)
                .Where(p => riscoIdList.Contains(p.TipoProduto.RiscoId))
                .ToListAsync();
        }

        #region metodos privados
        private async Task<List<Produto>> ObterProdutosComTipoAsync()
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(p => p.TipoProduto)
                    .ThenInclude(tp => tp.Risco)
                .ToListAsync();
        }
        #endregion        
    }
}
