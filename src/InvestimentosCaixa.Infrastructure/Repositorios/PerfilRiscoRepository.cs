using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class PerfilRiscoRepository : Repository<LogTelemetria>, IPerfilRiscoRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const string PerfilPontuacaoVolumeCache = "PerfilPontuacaoVolumeCache";
        private const string PerfilPontuacaoFrequenciaCache = "PerfilPontuacaoFrequenciaCache";
        private const string PerfilPontuacaoRiscoCache = "PerfilPontuacaoRiscoCache";
        private const string PerfilClassificacaoCache = "PerfilClassificacaoCache";
        public PerfilRiscoRepository(InvestimentosCaixaDbContext context, IMemoryCache memoryCache) : base(context) {
            _memoryCache = memoryCache;
        }

        public async Task<PerfilRisco?> ObterComRiscoPorNome(string nome)
        {
            return await _context.Set<PerfilRisco>().Include(x => x.RelPerfilRiscoList).AsNoTracking().FirstOrDefaultAsync(x => x.Nome == nome);
        }

        public async Task<PerfilPontuacaoVolume?> ObterPerfilPontuacaoVolume(decimal volumeInvestido)
        {
            var list = await _memoryCache.GetOrCreateAsync(PerfilPontuacaoVolumeCache, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return await _context.Set<PerfilPontuacaoVolume>()
                    .AsNoTracking()
                    .ToListAsync();
            });

            return list.FirstOrDefault(x =>
                x.MinValor <= volumeInvestido &&
                x.MaxValor >= volumeInvestido);
        }

        public async Task<PerfilPontuacaoFrequencia?> ObterPerfilPontuacaoFrequencia(int totalSimulacoes)
        {
            var list = await _memoryCache.GetOrCreateAsync(PerfilPontuacaoFrequenciaCache, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return await _context.Set<PerfilPontuacaoFrequencia>()
                    .AsNoTracking()
                    .ToListAsync();
            });

            return list.FirstOrDefault(x =>
                x.MinQtd <= totalSimulacoes &&
                x.MaxQtd >= totalSimulacoes);
        }

        public async Task<List<PerfilPontuacaoRisco>> ObterPerfilPontuacaoRiscoPorRiscos(List<int> riscoIdList)
        {
            var list = await _memoryCache.GetOrCreateAsync(PerfilPontuacaoRiscoCache, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return await _context.Set<PerfilPontuacaoRisco>()
                    .AsNoTracking()
                    .ToListAsync();
            });

            return list.Where(x => riscoIdList.Contains(x.RiscoId)).ToList();
        }

        public async Task<PerfilClassificacao?> ObterPerfilClassificacaoPorPontuacao(int pontuacaoCliente)
        {
            var list = await _memoryCache.GetOrCreateAsync(PerfilClassificacaoCache, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return await _context.Set<PerfilClassificacao>()
                    .Include(x => x.PerfilRisco)
                    .AsNoTracking()
                    .ToListAsync();
            });

            return list.FirstOrDefault(x =>
                x.MinPontuacao <= pontuacaoCliente &&
                x.MaxPontuacao >= pontuacaoCliente);
        }
    }
}
