using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class PerfilRiscoRepository : Repository<LogTelemetria>, IPerfilRiscoRepository
    {
        public PerfilRiscoRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<PerfilRisco?> ObterComRiscoPorNome(string nome)
        {
            return await _context.Set<PerfilRisco>().Include(x => x.RelPerfilRiscoList).AsNoTracking().FirstOrDefaultAsync(x => x.Nome == nome);
        }

        public async Task<PerfilPontuacaoVolume?> ObterPerfilPontuacaoVolume(decimal volumeInvestido)
        {
            return await _context.Set<PerfilPontuacaoVolume>().AsNoTracking().FirstOrDefaultAsync(x => x.MinValor <= volumeInvestido && x.MaxValor >= volumeInvestido);
        }

        public async Task<PerfilPontuacaoFrequencia?> ObterPerfilPontuacaoFrequencia(int totalSimulacoes)
        {
            return await _context.Set<PerfilPontuacaoFrequencia>().AsNoTracking().FirstOrDefaultAsync(x => x.MinQtd <= totalSimulacoes && x.MaxQtd >= totalSimulacoes);
        }

        public async Task<List<PerfilPontuacaoRisco>> ObterPerfilPontuacaoRiscoPorRiscos(List<int> riscoIdList)
        {
            return await _context.Set<PerfilPontuacaoRisco>().AsNoTracking().Where(x => riscoIdList.Contains(x.RiscoId)).ToListAsync();
        }

        public async Task<PerfilClassificacao?> ObterPerfilClassificacaoPorPontuacao(int pontuacaoCliente)
        {
            return await _context.Set<PerfilClassificacao>().Include(x => x.PerfilRisco).AsNoTracking().FirstOrDefaultAsync(x => x.MinPontuacao <= pontuacaoCliente && x.MaxPontuacao >= pontuacaoCliente);
        }
    }
}
