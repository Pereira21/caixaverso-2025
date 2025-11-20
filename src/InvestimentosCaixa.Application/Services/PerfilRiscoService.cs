using AutoMapper;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Resources;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;

namespace InvestimentosCaixa.Application.Services
{
    public class PerfilRiscoService : BaseService, IPerfilRiscoService
    {
        private readonly ILogger<PerfilRiscoService> _logger;
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IPerfilRiscoRepository _perfilRiscoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;

        public PerfilRiscoService(ILogger<PerfilRiscoService> logger, INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ISimulacaoRepository simulacaoRepository, IPerfilRiscoRepository perfilRiscoRepository, 
            IProdutoRepository produtoRepository, IInvestimentoRepository investimentoRepository) : base(notificador, mapper, unitOfWork)
        {
            _logger = logger;
            _simulacaoRepository = simulacaoRepository;
            _perfilRiscoRepository = perfilRiscoRepository;
            _produtoRepository = produtoRepository;
            _investimentoRepository = investimentoRepository;
        }

        public async Task<IEnumerable<ProdutoRecomendadoResponse>> ObterProdutosRecomendadosPorPerfilAsync(string perfil, int pagina, int tamanhoPagina)
        {
            var perfilRisco = await _perfilRiscoRepository.ObterComRiscoPorNomeAsync(perfil);
            if (perfilRisco == null)
            {
                _logger.LogWarning($"Perfil solicitado para obter produtos recomendados não existe!. Perfil: {perfil}!");
                Notificar(Mensagens.PerfilNaoEncontrado);
                return null;
            }

            var riscosVinculadosPerfilRisco = perfilRisco.RelPerfilRiscoList.Select(x => x.RiscoId).Distinct().ToList();
            var produtosRecomendados = await _produtoRepository.ObterPaginadoPorRiscoAsync(riscosVinculadosPerfilRisco, pagina, tamanhoPagina);

            return _mapper.Map<List<ProdutoRecomendadoResponse>>(produtosRecomendados);
        }

        public async Task<PerfilRiscoResponse?> ObterPorClienteIdAsync(int clienteId)
        {
            return await DiagnosticarPerfilRisco(clienteId);
        }

        #region metodos privados
        /// <summary>
        /// Obtém o Perfil de Risco do cliente a partir do seu histórico de investimentos ou simulações
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        private async Task<PerfilRiscoResponse?> DiagnosticarPerfilRisco(int clienteId)
        {
            int pontuacaoCliente = 0;
            var simulacoesPorCliente = new List<Simulacao>();

            var investimentosPorCliente = await _investimentoRepository.ObterComProdutoPorClienteIdAsync(clienteId);
            bool temInvestimentos = investimentosPorCliente != null && investimentosPorCliente.Count != 0;
            if (!temInvestimentos) // caso não haja investimentos, procuro simulações por cliente
            {
                simulacoesPorCliente = await _simulacaoRepository.ObterComProdutoPorClienteIdAsync(clienteId);
                if (simulacoesPorCliente == null || !simulacoesPorCliente.Any())
                {
                    _logger.LogWarning($"Cliente não possui investimentos para determinar Perfil de Risco!. Cliente: {clienteId}!");
                    Notificar(Mensagens.ClienteSemInvestimentosDeterminarPerfilRisco);
                    return null;
                }
            }

            decimal valorInvestido = temInvestimentos ? investimentosPorCliente.Sum(x => x.Valor) : simulacoesPorCliente.Sum(x => x.ValorInvestido);
            pontuacaoCliente += await ObtemScoreClienteVolume(valorInvestido);

            int quantidadeMovimentacoes = temInvestimentos ? investimentosPorCliente.Count() : simulacoesPorCliente.Count();
            pontuacaoCliente += await ObtemScoreClienteFrequencia(quantidadeMovimentacoes);

            var riscosMovimentadosAgrupados = temInvestimentos ?
                                             investimentosPorCliente.GroupBy(x => x.Produto.TipoProduto.RiscoId)
                                                    .Select(g => new RiscoAgrupadoDto()
                                                    {
                                                        RiscoId = g.Key,
                                                        Quantidade = g.Count()
                                                    }).ToList() :
                                              simulacoesPorCliente.GroupBy(x => x.Produto.TipoProduto.RiscoId)
                                                     .Select(g => new RiscoAgrupadoDto()
                                                     {
                                                         RiscoId = g.Key,
                                                         Quantidade = g.Count()
                                                     }).ToList();

            pontuacaoCliente += await ObtemScoreClienteRisco(riscosMovimentadosAgrupados);

            PerfilClassificacao perfilClassificacao = await ObtemPerfilClassificacao(pontuacaoCliente);

            if (_notificador.TemNotificacao())
                return null;

            return new PerfilRiscoResponse
            {
                ClienteId = clienteId,
                Pontuacao = pontuacaoCliente,
                Perfil = perfilClassificacao.PerfilRisco.Nome,
                Descricao = perfilClassificacao.PerfilRisco.Descricao
            };
        }

        /// <summary>
        /// Obtém o score do cliente baseado no volume investido ou simulado
        /// </summary>
        /// <param name="valorInvestido">Valor total investido ou simulado pelo cliente</param>
        /// <returns></returns>
        private async Task<int> ObtemScoreClienteVolume(decimal valorInvestido)
        {
            var perfilPontuacaoVolume = await _perfilRiscoRepository.ObterPerfilPontuacaoVolumeAsync(valorInvestido);

            if (perfilPontuacaoVolume != null)
                return perfilPontuacaoVolume.Pontos;

            return 0;
        }

        /// <summary>
        /// Obtém o score do cliente baseado na frequência de investimentos ou simulações
        /// </summary>
        /// <param name="quantidadeMovimentacoes">Quantidade totall investimentos ou simulações do cliente</param>
        /// <returns></returns>
        private async Task<int> ObtemScoreClienteFrequencia(int quantidadeMovimentacoes)
        {
            var perfilPontuacaoFrequencia = await _perfilRiscoRepository.ObterPerfilPontuacaoFrequenciaAsync(quantidadeMovimentacoes);
            if (perfilPontuacaoFrequencia != null)
                return perfilPontuacaoFrequencia.Pontos;

            return 0;
        }

        /// <summary>
        /// Obtém o score do cliente baseado no risco dos produtos investidos ou simulados
        /// </summary>
        /// <param name="riscosMovimentadosAgrupados">Através do risco do produto investido ou simulado, tenho um agrupamento deles com a quantidade</param>
        /// <returns></returns>
        private async Task<int> ObtemScoreClienteRisco(List<RiscoAgrupadoDto> riscosMovimentadosAgrupados)
        {
            int scoreTotalRiscoProdutos = 0;

            var perfilPontuacaoRisco = await _perfilRiscoRepository.ObterPerfilPontuacaoRiscoPorRiscosAsync(riscosMovimentadosAgrupados.Select(x => x.RiscoId).Distinct().ToList());

            if (perfilPontuacaoRisco != null && perfilPontuacaoRisco.Any())
            {
                // Ordeno decrescente pelo risco de maior peso pra ser a base do cálculo
                perfilPontuacaoRisco = perfilPontuacaoRisco.OrderByDescending(x => x.PontosBase).ToList();

                bool naoAdicionarPontuacaoBase = false;
                foreach (var risco in perfilPontuacaoRisco)
                {
                    int quantidadeRisco = riscosMovimentadosAgrupados.FirstOrDefault(x => x.RiscoId == risco.Id).Quantidade;
                    int totalRisco = 0;

                    if (naoAdicionarPontuacaoBase)
                    {
                        totalRisco = 0;

                        int incremento = (int)(risco.PontosBase * (risco.Multiplicador - 1));
                        totalRisco += incremento * (quantidadeRisco);
                    }
                    else
                    {
                        totalRisco = risco.PontosBase;

                        if (quantidadeRisco > 1)
                        {
                            int incremento = (int)(risco.PontosBase * (risco.Multiplicador - 1));
                            totalRisco += incremento * (quantidadeRisco - 1);
                        }
                    }

                    // aplica teto máximo
                    totalRisco = Math.Min(totalRisco, risco.PontosMaximos);

                    scoreTotalRiscoProdutos += totalRisco;
                    naoAdicionarPontuacaoBase = true;
                }
            }

            return scoreTotalRiscoProdutos;
        }

        /// <summary>
        /// Obtém a classificação do perfil de risco baseado na pontuação total do cliente
        /// </summary>
        /// <param name="pontuacaoCliente">Pontuação final do cliente após análises do histórico de investimentos/simulações</param>
        /// <returns></returns>
        private async Task<PerfilClassificacao> ObtemPerfilClassificacao(int pontuacaoCliente)
        {
            var perfilClassificacaoDto = await _perfilRiscoRepository.ObterPerfilClassificacaoPorPontuacaoAsync(pontuacaoCliente);
            if (perfilClassificacaoDto == null)
            {
                _logger.LogWarning($"Não foi encontrado um Perfil Risco para a pontuação {pontuacaoCliente}!");
                Notificar(Mensagens.NaoFoiPossivelDeterminarPerfilRisco);
            }

            return _mapper.Map<PerfilClassificacao>(perfilClassificacaoDto);
        }
        #endregion
    }
}
