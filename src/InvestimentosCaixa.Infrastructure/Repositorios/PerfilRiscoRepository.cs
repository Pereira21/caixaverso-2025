using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class PerfilRiscoRepository : Repository<LogTelemetria>, IPerfilRiscoRepository
    {
        public PerfilRiscoRepository(InvestimentosCaixaDbContext context) : base(context) { }

        public async Task<PerfilPontuacaoVolume?> ObterPerfilPontuacaoVolume(decimal volumeInvestido)
        {
            return await _context.Set<PerfilPontuacaoVolume>().AsNoTracking().FirstOrDefaultAsync(x => x.MinValor <= volumeInvestido && x.MaxValor >= volumeInvestido);
        }
    }
}
