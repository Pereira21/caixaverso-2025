using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.Helpers;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace InvestimentosCaixa.Infrastructure.Repositorios
{
    public class PerfilRiscoRepository : Repository<PerfilRisco>, IPerfilRiscoRepository
    {
        private const string PerfilPontuacaoVolumeCache = "PerfilPontuacaoVolumeCache";
        private const string PerfilPontuacaoFrequenciaCache = "PerfilPontuacaoFrequenciaCache";
        private const string PerfilPontuacaoRiscoCache = "PerfilPontuacaoRiscoCache";
        private const string PerfilClassificacaoCache = "PerfilClassificacaoCache";
        public PerfilRiscoRepository(InvestimentosCaixaDbContext context, IDistributedCache distributedCache) : base(context, distributedCache) { }

        public async Task<PerfilRisco?> ObterComRiscoPorNomeAsync(string nome)
        {
            return await _context.Set<PerfilRisco>().Include(x => x.RelPerfilRiscoList).AsNoTracking().FirstOrDefaultAsync(x => x.Nome == nome);
        }

        public async Task<PerfilPontuacaoVolumeDto?> ObterPerfilPontuacaoVolumeAsync(decimal volumeInvestido)
        {
            var list = await RedisHelper.GetOrSetCacheAsync<List<PerfilPontuacaoVolumeDto>>(_distributedCache,
                PerfilPontuacaoVolumeCache,
                async () =>
                {
                    return await _context.Set<PerfilPontuacaoVolume>()
                        .AsNoTracking()
                        .Select(p => new PerfilPontuacaoVolumeDto
                        {
                            Id = p.Id,
                            MinValor = p.MinValor,
                            MaxValor = p.MaxValor,
                            Pontos = p.Pontos
                        })
                        .ToListAsync();
                }
            );

            return list.FirstOrDefault(x =>
                x.MinValor <= volumeInvestido &&
                x.MaxValor >= volumeInvestido);
        }

        public async Task<PerfilPontuacaoFrequenciaDto?> ObterPerfilPontuacaoFrequenciaAsync(int totalSimulacoes)
        {
            var list = await RedisHelper.GetOrSetCacheAsync<List<PerfilPontuacaoFrequenciaDto>>(
                _distributedCache,
                PerfilPontuacaoFrequenciaCache,
                async () =>
                {
                    return await _context.Set<PerfilPontuacaoFrequencia>()
                        .Select(p => new PerfilPontuacaoFrequenciaDto
                        {
                            Id = p.Id,
                            MinQtd = p.MinQtd,
                            MaxQtd = p.MaxQtd,
                            Pontos = p.Pontos
                        })
                        .ToListAsync();
                }
            );

            return list.FirstOrDefault(x =>
                x.MinQtd <= totalSimulacoes &&
                x.MaxQtd >= totalSimulacoes);
        }

        public async Task<List<PerfilPontuacaoRiscoDto>> ObterPerfilPontuacaoRiscoPorRiscosAsync(List<int> riscoIdList)
        {
            var list = await RedisHelper.GetOrSetCacheAsync<List<PerfilPontuacaoRiscoDto>>(
                _distributedCache,
                PerfilPontuacaoRiscoCache,
                async () =>
                {
                    return await _context.Set<PerfilPontuacaoRisco>()
                        .Select(p => new PerfilPontuacaoRiscoDto
                        {
                            Id = p.Id,
                            PontosBase = p.PontosBase,
                            Multiplicador = p.Multiplicador,
                            PontosMaximos = p.PontosMaximos,
                            RiscoId = p.RiscoId
                        })
                        .ToListAsync();
                }
            );

            return list.Where(x => riscoIdList.Contains(x.RiscoId)).ToList();
        }

        public async Task<PerfilClassificacaoDto?> ObterPerfilClassificacaoPorPontuacaoAsync(int pontuacaoCliente)
        {
            var list = await RedisHelper.GetOrSetCacheAsync<List<PerfilClassificacaoDto>>(
                _distributedCache,
                PerfilClassificacaoCache,
                async () =>
                {
                    return await _context.Set<PerfilClassificacao>()
                        .Include(x => x.PerfilRisco)
                        .Select(p => new PerfilClassificacaoDto
                        {
                            Id = p.Id,
                            MinPontuacao = p.MinPontuacao,
                            MaxPontuacao = p.MaxPontuacao,
                            PerfilRisco = new PerfilRiscoDto
                            {
                                Id = p.Id,
                                Nome = p.PerfilRisco.Nome,
                                Descricao = p.PerfilRisco.Descricao
                            }
                        })
                        .ToListAsync();
                }
            );

            return list.FirstOrDefault(x =>
                x.MinPontuacao <= pontuacaoCliente &&
                x.MaxPontuacao >= pontuacaoCliente);
        }
    }
}