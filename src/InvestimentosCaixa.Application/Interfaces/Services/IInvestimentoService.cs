
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface IInvestimentoService
    {
        /// <summary>
        /// Obter investimentos de um cliente específico com os dados do produto associado, paginados
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userEmail"></param>
        /// <param name="clienteId"></param>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<List<InvestimentoResponse>> ObterPorClienteIdAsync(Guid userId, string userEmail, int clienteId, int pagina, int tamanhoPagina);
    }
}
