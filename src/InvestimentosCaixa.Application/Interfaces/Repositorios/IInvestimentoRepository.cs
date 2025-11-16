using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IInvestimentoRepository : IRepository<Investimento>
    {
        Task<List<Investimento>> ObterComProdutoPorClienteId(int clienteId);
    }
}
