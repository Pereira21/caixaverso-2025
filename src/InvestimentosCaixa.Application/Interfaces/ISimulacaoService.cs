using InvestimentosCaixa.Application.DTO;

namespace InvestimentosCaixa.Application.Interfaces
{
    public interface ISimulacaoService
    {        
        Task<SimularInvestimentoResponseDTO> SimularInvestimento(SimularInvestimentoRequestDTO request);

        Task<List<SimulacaoResponseDTO>> ObterHistorico();
    }
}
