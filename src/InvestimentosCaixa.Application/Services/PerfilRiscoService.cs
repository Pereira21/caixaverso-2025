using AutoMapper;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Domain.Entidades;

namespace InvestimentosCaixa.Application.Services
{
    public class PerfilRiscoService : BaseService, IPerfilRiscoService
    {
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IPerfilRiscoRepository _perfilRiscoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IInvestimentoRepository _investimentoRepository;

        public PerfilRiscoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ISimulacaoRepository simulacaoRepository, IPerfilRiscoRepository perfilRiscoRepository, IProdutoRepository produtoRepository,
            IInvestimentoRepository investimentoRepository) : base(notificador, mapper, unitOfWork)
        {
            _simulacaoRepository = simulacaoRepository;
            _perfilRiscoRepository = perfilRiscoRepository;
            _produtoRepository = produtoRepository;
            _investimentoRepository = investimentoRepository;
        }

        public async Task<IEnumerable<ProdutoRecomendadoResponse>> ObterProdutosRecomendadosPorPerfil(string perfil)
        {
            var perfilRisco = await _perfilRiscoRepository.ObterComRiscoPorNome(perfil);
            if (perfilRisco == null)
            {
                Notificar("Perfil não encontrado!");
                return null;
            }

            var riscosVinculadosPerfilRisco = perfilRisco.RelPerfilRiscoList.Select(x => x.RiscoId).Distinct().ToList();
            var produtosRecomendados = await _produtoRepository.ObterPorRiscoAsync(riscosVinculadosPerfilRisco);

            return _mapper.Map<List<ProdutoRecomendadoResponse>>(produtosRecomendados);
        }

        public async Task<PerfilRiscoResponse?> ObterPorClienteId(int clienteId)
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

            var investimentosPorCliente = await _investimentoRepository.ObterComProdutoPorClienteId(clienteId);
            bool temInvestimentos = investimentosPorCliente != null && investimentosPorCliente.Count != 0;
            if (!temInvestimentos) // caso não haja investimentos, procuro simulações por cliente
            {
                simulacoesPorCliente = await _simulacaoRepository.ObterComProdutoPorClienteId(clienteId);
                if (simulacoesPorCliente == null || !simulacoesPorCliente.Any())
                {
                    Notificar("Cliente não possui investimentos para determinar um Perfil de Risco!");
                    return null;
                }
            }

            decimal valorInvestido = temInvestimentos ? investimentosPorCliente.Sum(x => x.Valor) : simulacoesPorCliente.Sum(x => x.ValorInvestido);
            pontuacaoCliente += await ObtemScoreClienteVolume(valorInvestido);

            int quantidadeMovimentacoes = temInvestimentos ? investimentosPorCliente.Count() : simulacoesPorCliente.Count();
            pontuacaoCliente += await ObtemScoreClienteFrequencia(quantidadeMovimentacoes);

            var riscosMovimentadosAgrupados = temInvestimentos ?
                                             investimentosPorCliente.GroupBy(x => x.Produto.TipoProduto.RiscoId)
                                                    .Select(g => new RiscoAgrupadoDTO()
                                                    {
                                                        RiscoId = g.Key,
                                                        Quantidade = g.Count()
                                                    }).ToList() :
                                              simulacoesPorCliente.GroupBy(x => x.Produto.TipoProduto.RiscoId)
                                                     .Select(g => new RiscoAgrupadoDTO()
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
            var perfilPontuacaoVolume = await _perfilRiscoRepository.ObterPerfilPontuacaoVolume(valorInvestido);

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
            var perfilPontuacaoFrequencia = await _perfilRiscoRepository.ObterPerfilPontuacaoFrequencia(quantidadeMovimentacoes);
            if (perfilPontuacaoFrequencia != null)
                return perfilPontuacaoFrequencia.Pontos;

            return 0;
        }

        /// <summary>
        /// Obtém o score do cliente baseado no risco dos produtos investidos ou simulados
        /// </summary>
        /// <param name="riscosMovimentadosAgrupados">Através do risco do produto investido ou simulado, tenho um agrupamento deles com a quantidade</param>
        /// <returns></returns>
        private async Task<int> ObtemScoreClienteRisco(List<RiscoAgrupadoDTO> riscosMovimentadosAgrupados)
        {
            int scoreTotalRiscoProdutos = 0;
            var perfilPontuacaoRisco = await _perfilRiscoRepository.ObterPerfilPontuacaoRiscoPorRiscos(riscosMovimentadosAgrupados.Select(x => x.RiscoId).Distinct().ToList());
            if (perfilPontuacaoRisco != null && perfilPontuacaoRisco.Any())
            {
                foreach (var risco in perfilPontuacaoRisco)
                {
                    int quantidadeRisco = riscosMovimentadosAgrupados.Count(x => x.RiscoId == risco.Id);

                    int totalRisco = risco.PontosBase;

                    if (quantidadeRisco > 1)
                    {
                        int incremento = (int)(risco.PontosBase * (risco.Multiplicador - 1));
                        totalRisco += incremento * (quantidadeRisco - 1);
                    }

                    // aplica teto máximo
                    totalRisco = Math.Min(totalRisco, risco.PontosMaximos);

                    scoreTotalRiscoProdutos += totalRisco;
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
            var perfilClassificacao = await _perfilRiscoRepository.ObterPerfilClassificacaoPorPontuacao(pontuacaoCliente);
            if (perfilClassificacao == null)
            {
                Notificar("Não foi possível determinar o Perfil de Risco do cliente!");
            }

            return perfilClassificacao;
        }
        #endregion
    }
}
