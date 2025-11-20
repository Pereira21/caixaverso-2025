using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface IPerfilRiscoService
    {
        /// <summary>
        /// Obter perfil de risco por clienteId
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        Task<PerfilRiscoResponse?> ObterPorClienteIdAsync(int clienteId);

        /// <summary>
        /// Obter produtos recomendados conforme perfil de risco
        /// </summary>
        /// <param name="perfil"></param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<IEnumerable<ProdutoRecomendadoResponse>> ObterProdutosRecomendadosPorPerfilAsync(string perfil, int pagina, int tamanhoPagina);
    }
}
