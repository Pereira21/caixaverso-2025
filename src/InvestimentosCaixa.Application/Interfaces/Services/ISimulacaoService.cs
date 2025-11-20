using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ISimulacaoService
    {
        /// <summary>
        /// Simular investimento conforme parâmetros fornecidos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SimularInvestimentoResponse> SimularInvestimentoAsync(SimularInvestimentoRequest request);

        /// <summary>
        /// Obter histórico de simulações paginadas
        /// </summary>
        /// <param name="userId">Id do usuário que está realizando a consulta</param>
        /// <param name="userEmail">E-mail do usuário que está realizando a consulta</param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<SimulacaoResponseDTO>> ObterHistoricoAsync(Guid userId, string userEmail, int pagina, int tamanhoPagina);

        /// <summary>
        /// Obter simulações agrupadas por produto e dia, paginadas
        /// </summary>
        /// <param name="userId">Id do usuário que está realizando a consulta</param>
        /// <param name="userEmail">E-mail do usuáiro que está realizando a consulta</param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<SimulacaoPorProdutoDiaResponse>> ObterPorProdutoDiaAsync(Guid userId, string userEmail, int pagina, int tamanhoPagina);
    }
}
