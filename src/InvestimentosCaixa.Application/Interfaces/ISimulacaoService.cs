using InvestimentosCaixa.Application.DTO;

namespace InvestimentosCaixa.Application.Interfaces
{
    public interface ISimulacaoService
    {
        Task<SimularInvestimentoDTO> SimularInvestimento(int clienteId, decimal valor, short prazoMeses, string tipoProduto);
    }
}
