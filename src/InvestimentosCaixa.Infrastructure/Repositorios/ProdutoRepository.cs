using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.Helpers;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private const string ProdutosCache = "ProdutosCache";
        public ProdutoRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<ProdutoDto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto)
        {
            List<ProdutoDto> list = await RedisHelper.GetOrSetCacheAsync(_distributedCache,
                ProdutosCache,
                ObterProdutosComTipoAsync
            );

            return list
                .FirstOrDefault(p => p.TipoProduto.Nome == tipoProduto && p.PrazoMinimoMeses <= prazoMeses);
        }

        public async Task<List<Produto>> ObterPaginadoPorRiscoAsync(List<int> riscoIdList, int pagina, int tamanhoPagina)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.TipoProduto)
                    .ThenInclude(x => x.Risco)
                .Where(p => riscoIdList.Contains(p.TipoProduto.RiscoId))
                .OrderBy(p => p.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
        }

        public async Task<List<TipoProduto>> ObterTipoProdutoComProdutosAsync()
        {
            return await _context.TiposProduto.AsNoTracking()
                .Include(x => x.Produtos)
                .Include(x => x.Risco)
                .ToListAsync();
        }

        #region metodos privados
        private async Task<List<ProdutoDto>> ObterProdutosComTipoAsync()
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(p => p.TipoProduto)
                .ThenInclude(tp => tp.Risco)
                .Select(p => new ProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PrazoMinimoMeses = p.PrazoMinimoMeses,
                    RentabilidadeAnual = p.RentabilidadeAnual,
                    TipoProduto = new TipoProdutoDto
                    {
                        Id = p.TipoProduto.Id,
                        Nome = p.TipoProduto.Nome,
                        Risco = new RiscoDto
                        {
                            Id = p.TipoProduto.Risco.Id,
                            Nome = p.TipoProduto.Risco.Nome
                        }
                    }
                })
                .ToListAsync();
        }
        #endregion        
    }
}
