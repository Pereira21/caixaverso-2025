using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ISimulacaoService
    {        
        Task<SimularInvestimentoResponse> SimularInvestimento(SimularInvestimentoRequest request);

        Task<List<SimulacaoResponseDTO>> ObterHistorico(Guid userId, string userEmail, int pagina, int tamanhoPagina);
        Task<List<SimulacaoPorProdutoDiaResponse>> ObterPorProdutoDiaAsync(Guid userId, string userEmail);
    }
}
