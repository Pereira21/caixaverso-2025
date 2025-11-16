using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;

namespace InvestimentosCaixa.Application.Services
{
    public class PerfilRiscoService : BaseService, IPerfilRiscoService
    {
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly IPerfilRiscoRepository _perfilRiscoRepository;

        public PerfilRiscoService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork, ISimulacaoRepository simulacaoRepository, IPerfilRiscoRepository perfilRiscoRepository) :
            base(notificador, mapper, unitOfWork)
        {
            _simulacaoRepository = simulacaoRepository;
            _perfilRiscoRepository = perfilRiscoRepository;
        }

        public async Task<PerfilRiscoResponse?> ObterPorClienteId(int clienteId)
        {
            return await DiagnosticarPerfilRisco(clienteId);
        }

        public Task<IEnumerable<ProdutoRecomendadoResponse>> ObterProdutosRecomendadosPorPerfil(string perfil)
        {
            //var perfilRisco = _perfilRiscoRepository.ObterPorNome(perfil);
            //if(perfilRisco == null)
            //{
            //    Notificar("Perfil não encontrado!");
                return null;
            //}
        }

        #region metodos privados
        private async Task<PerfilRiscoResponse?> DiagnosticarPerfilRisco(int clienteId)
        {
            int pontuacaoCliente = 0;

            var simulacoesPorCliente = await _simulacaoRepository.ObterComProdutoPorClienteId(clienteId);

            if (simulacoesPorCliente == null || !simulacoesPorCliente.Any())
            {
                Notificar("Cliente não possui simulações para determinar um Perfil de Risco!");
                return null;
            }

            var perfilPontuacaoVolume = await _perfilRiscoRepository.ObterPerfilPontuacaoVolume(simulacoesPorCliente.Sum(x => x.ValorInvestido));

            if (perfilPontuacaoVolume != null)
            {
                pontuacaoCliente += perfilPontuacaoVolume.Pontos;
            }

            var perfilPontuacaoFrequencia = await _perfilRiscoRepository.ObterPerfilPontuacaoFrequencia(simulacoesPorCliente.Count());

            if (perfilPontuacaoFrequencia != null)
                pontuacaoCliente += perfilPontuacaoFrequencia.Pontos;

            var riscosSimuladosAgrupados = simulacoesPorCliente.GroupBy(x => x.Produto.TipoProduto.RiscoId)
                                                     .Select(g => new
                                                     {
                                                         RiscoId = g.Key,
                                                         Quantidade = g.Count()
                                                     }).ToList();

            var perfilPontuacaoRisco = await _perfilRiscoRepository.ObterPerfilPontuacaoRiscoPorRiscos(riscosSimuladosAgrupados.Select(x => x.RiscoId).Distinct().ToList());

            if (perfilPontuacaoRisco != null && perfilPontuacaoRisco.Any())
            {
                //int pontuacaoRisco = 0;

                foreach (var risco in perfilPontuacaoRisco)
                {
                    int quantidadeRisco = riscosSimuladosAgrupados.Count(x => x.RiscoId == risco.Id);

                    int totalRisco = risco.PontosBase;

                    if (quantidadeRisco > 1)
                    {
                        int incremento = (int)(risco.PontosBase * (risco.Multiplicador - 1));
                        totalRisco += incremento * (quantidadeRisco - 1);
                    }

                    // aplica teto máximo
                    totalRisco = Math.Min(totalRisco, risco.PontosMaximos);

                    pontuacaoCliente += totalRisco;
                }
            }

            var perfilClassificacao = await _perfilRiscoRepository.ObterPerfilClassificacaoPorPontuacao(pontuacaoCliente);

            if (perfilClassificacao == null)
            {
                Notificar("Não foi possível determinar o Perfil de Risco do cliente!");
            }

            return new PerfilRiscoResponse
            {
                ClienteId = clienteId,
                Pontuacao = pontuacaoCliente,
                Perfil = perfilClassificacao.PerfilRisco.Nome,
                Descricao = perfilClassificacao.PerfilRisco.Descricao
            };
        }
        #endregion
    }
}
