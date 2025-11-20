using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        /// <summary>
        /// Obter produto adequado conforme prazo e tipo
        /// </summary>
        /// <param name="prazoMeses"></param>
        /// <param name="tipoProduto"></param>
        /// <returns></returns>
        Task<ProdutoDto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto);

        /// <summary>
        /// Obter produtos paginados conforme lista de riscos
        /// </summary>
        /// <param name="riscoIdList"></param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<Produto>> ObterPaginadoPorRiscoAsync(List<int> riscoIdList, int pagina, int tamanhoPagina);

        /// <summary>
        /// Obter tipos de produtos com seus respectivos produtos
        /// </summary>
        /// <returns></returns>
        Task<List<TipoProduto>> ObterTipoProdutoComProdutosAsync();
    }
}
