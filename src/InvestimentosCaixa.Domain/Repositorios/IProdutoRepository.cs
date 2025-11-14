using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Domain.Interfaces;

namespace InvestimentosCaixa.Domain.Repositorios
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<Produto?> ObterAdequadoAsync(short prazoMeses, string tipoProduto, CancellationToken cancellationToken = default);
    }
}
