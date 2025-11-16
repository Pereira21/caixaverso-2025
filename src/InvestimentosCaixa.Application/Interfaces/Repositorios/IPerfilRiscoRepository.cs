
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IPerfilRiscoRepository
    {
        Task<PerfilPontuacaoVolume?> ObterPerfilPontuacaoVolume(decimal volumeInvestido);
    }
}
