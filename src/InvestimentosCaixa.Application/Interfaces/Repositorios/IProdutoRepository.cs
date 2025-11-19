using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<ProdutoDto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto);
        Task<List<Produto>> ObterPaginadoPorRiscoAsync(List<int> riscoIdList, int pagina, int tamanhoPagina);
        Task<List<TipoProduto>> ObterTipoProdutoComProdutosAsync();
    }
}
