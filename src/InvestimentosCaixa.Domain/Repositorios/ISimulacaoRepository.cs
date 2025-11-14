using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Domain.Interfaces;

namespace InvestimentosCaixa.Domain.Repositorios
{
    public interface ISimulacaoRepository : IRepository<Simulacao>
    {
        Task<List<Simulacao>> ObterTodosComProdutoAsync();
    }
}
