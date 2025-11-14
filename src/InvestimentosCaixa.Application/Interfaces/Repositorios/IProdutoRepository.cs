using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<Produto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto, CancellationToken cancellationToken = default);
    }
}
