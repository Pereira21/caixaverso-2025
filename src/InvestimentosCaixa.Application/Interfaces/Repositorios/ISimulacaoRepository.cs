using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface ISimulacaoRepository : IRepository<Simulacao>
    {
        /// <summary>
        /// Obter todas as simulações paginadas com os dados do produto associado
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<Simulacao>> ObterTodosPaginadoComProdutoAsync(int pagina, int tamanhoPagina);

        /// <summary>
        /// Obter simulações de um cliente específico com os dados do produto associado
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        Task<List<Simulacao>> ObterComProdutoPorClienteIdAsync(int clienteId);

        /// <summary>
        /// Obter simulações agrupadas por produto e dia, paginadas
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<SimulacaoPorProdutoDiaResponse>> ObterPaginadoSimulacoesPorProdutoDiaAsync(int pagina, int tamanhoPagina);
    }
}
