using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<Produto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto);
        Task<List<Produto>> ObterPorRiscoAsync(List<int> riscoIdList);
    }
}
