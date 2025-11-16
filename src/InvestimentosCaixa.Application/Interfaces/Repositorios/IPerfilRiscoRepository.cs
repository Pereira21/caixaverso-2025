
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Interfaces.Repositorios
{
    public interface IPerfilRiscoRepository
    {
        Task<PerfilRisco?> ObterComRiscoPorNome(string nome);

        Task<PerfilPontuacaoVolume?> ObterPerfilPontuacaoVolume(decimal volumeInvestido);
        Task<PerfilPontuacaoFrequencia?> ObterPerfilPontuacaoFrequencia(int totalSimulacoes);
        Task<List<PerfilPontuacaoRisco>> ObterPerfilPontuacaoRiscoPorRiscos(List<int> riscoIdList);
        Task<PerfilClassificacao?> ObterPerfilClassificacaoPorPontuacao(int pontuacaoCliente);        
    }
}
