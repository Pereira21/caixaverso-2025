using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IInvestimentoRepository : IRepository<Investimento>
    {
        /// <summary>
        /// Obter investimentos de um cliente específico com os dados do produto associado
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        Task<List<Investimento>> ObterComProdutoPorClienteIdAsync(int clienteId);

        /// <summary>
        /// Obter investimentos de um cliente específico com os dados do produto associado, paginados
        /// </summary>
        /// <param name="clienteId"></param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<Investimento>> ObterPaginadoComProdutoPorClienteIdAsync(int clienteId, int pagina, int tamanhoPagina);
    }
}
