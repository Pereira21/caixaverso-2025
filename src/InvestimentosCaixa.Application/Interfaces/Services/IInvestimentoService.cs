
using InvestimentosCaixa.Application.DTO.Response;

namespace InvestimentosCaixa.Application.Interfaces.Services
{
    public interface IInvestimentoService
    {
        Task<List<InvestimentoResponse>> ObterPorClienteId(int clienteId);
    }
}
