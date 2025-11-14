using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface ISimulacaoService
    {        
        Task<SimularInvestimentoResponse> SimularInvestimento(SimularInvestimentoRequest request);

        Task<List<SimulacaoResponseDTO>> ObterHistorico();
        Task<List<SimulacaoPorProdutoDiaResponse>> ObterPorProdutoDiaAsync();
    }
}
