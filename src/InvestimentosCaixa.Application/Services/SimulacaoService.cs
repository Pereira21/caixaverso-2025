using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.Interfaces;

namespace InvestimentosCaixa.Application.Services
{
    public class SimulacaoService : ISimulacaoService
    {
        public SimulacaoService()
        {
        }

        public async Task<SimularInvestimentoDTO> SimularInvestimento(int clienteId, decimal valor, short prazoMeses, string tipoProduto)
        {
            return new SimularInvestimentoDTO
            {
                DataSimulacao = DateTime.UtcNow,
            };
        }
    }
}
