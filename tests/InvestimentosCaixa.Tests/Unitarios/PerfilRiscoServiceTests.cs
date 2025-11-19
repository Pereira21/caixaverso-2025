using AutoMapper;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Resources;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Tests.Unitarios
{
    public class PerfilRiscoServiceTests
    {
        private readonly Mock<ILogger<PerfilRiscoService>> _loggerMock;
        private readonly Mock<ISimulacaoRepository> _simulacaoRepositoryMock;
        private readonly Mock<IPerfilRiscoRepository> _perfilRiscoRepositoryMock;
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IInvestimentoRepository> _investimentoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Notificador _notificador;

        private PerfilRiscoService CreateService() =>
            new PerfilRiscoService(
                _loggerMock.Object,
                _notificador,
                _mapperMock.Object,
                _unitOfWorkMock.Object,
                _simulacaoRepositoryMock.Object,
                _perfilRiscoRepositoryMock.Object,
                _produtoRepositoryMock.Object,
                _investimentoRepositoryMock.Object
            );

        public PerfilRiscoServiceTests()
        {
            _loggerMock = new Mock<ILogger<PerfilRiscoService>>();
            _simulacaoRepositoryMock = new Mock<ISimulacaoRepository>();
            _perfilRiscoRepositoryMock = new Mock<IPerfilRiscoRepository>();
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _investimentoRepositoryMock = new Mock<IInvestimentoRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _notificador = new Notificador();
        }

        [Fact(DisplayName = "Deve notificar quando perfil inexiste ao obter produtos recomendados por perfil")]
        public async Task ObterProdutosRecomendadosPorPerfil_PerfilInexistente_DeveNotificar()
        {
            // Arrange
            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterComRiscoPorNome("Conservador"))
                .ReturnsAsync((PerfilRisco?)null);

            var service = CreateService();

            // Act
            var result = await service.ObterProdutosRecomendadosPorPerfil("Conservador");

            // Assert
            Assert.Null(result);
            Assert.Contains(_notificador.ObterNotificacoes(),
                n => n.Mensagem == Mensagens.PerfilNaoEncontrado);
        }

        [Fact(DisplayName = "Deve retornar lista mapeada quando obtem produtos recomendados por perfil")]
        public async Task ObterProdutosRecomendadosPorPerfil_Valido_DeveRetornarProdutos()
        {
            // Arrange
            var perfil = new PerfilRisco("Moderado", "PerfilModerado");
            perfil.AdicionarRisco(2);

            var produtos = new List<Produto>
            {
                new Produto(2, "CDB", 0.95m, 0)
            };

            var produtosDTO = new List<ProdutoRecomendadoResponse>
            {
                new ProdutoRecomendadoResponse { Nome = "CDB XPTO" }
            };

            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterComRiscoPorNome("Moderado"))
                .ReturnsAsync(perfil);

            _produtoRepositoryMock
                .Setup(r => r.ObterPorRiscoAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(produtos);

            _mapperMock
                .Setup(m => m.Map<List<ProdutoRecomendadoResponse>>(produtos))
                .Returns(produtosDTO);

            var service = CreateService();

            // Act
            var result = await service.ObterProdutosRecomendadosPorPerfil("Moderado");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("CDB XPTO", result.First().Nome);

            _perfilRiscoRepositoryMock.Verify(r => r.ObterComRiscoPorNome("Moderado"), Times.Once);
            _produtoRepositoryMock.Verify(r => r.ObterPorRiscoAsync(It.IsAny<List<int>>()), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ProdutoRecomendadoResponse>>(produtos), Times.Once);
        }


        [Fact(DisplayName = "Deve notificar quando não há investimentos nem simulações para obter perfilrisco")]
        public async Task ObterPorClienteId_SemInvestimentosSemSimulacoes_DeveNotificar()
        {
            // Arrange
            int clienteId = 123;

            _investimentoRepositoryMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(new List<Investimento>()); // nenhum investimento

            _simulacaoRepositoryMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(new List<Simulacao>()); // nenhuma simulação

            var service = CreateService();

            // Act
            var result = await service.ObterPorClienteId(clienteId);

            // Assert
            Assert.Null(result);
            Assert.Contains(_notificador.ObterNotificacoes(),
                n => n.Mensagem == Mensagens.ClienteSemInvestimentosDeterminarPerfilRisco);
        }

        [Fact(DisplayName = "Deve retornar perfil quando tiver investimentos no obter perfil por clienteId")]
        public async Task ObterPorClienteId_ComInvestimentos_DeveRetornarPerfil()
        {
            // Arrange
            int clienteId = 10;

            var tipoProduto = new TipoProduto("Tesouro Direto", 1, "Diária", "Tesouro Direto");
            var produto = new Produto(1, "Tesouro Direto", 0.1m, 1, tipoProduto);            
            var investimentos = new List<Investimento>
                {
                    new Investimento(1, 1, 1, 1000, 0.12m, DateTime.UtcNow, null, produto)
                };

            _investimentoRepositoryMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(investimentos);

            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoVolume(It.IsAny<decimal>()))
                .ReturnsAsync(new PerfilPontuacaoVolumeDto
                {
                    MinValor = 0.01m,
                    MaxValor = 5000,
                    Pontos = 10
                });

            // Score frequência
            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoFrequencia(1))
                .ReturnsAsync(new PerfilPontuacaoFrequenciaDto
                {
                    MinQtd = 0,
                    MaxQtd = 3,
                    Pontos = 10
                });

            // Score risco
            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoRiscoPorRiscos(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<PerfilPontuacaoRiscoDto>
                {
                        new PerfilPontuacaoRiscoDto
                        {
                            Id = 1,
                            PontosBase = 30,
                            PontosMaximos = 100,
                            Multiplicador = 1.5m
                        }
                });

            // Classificação final
            var classificacaoDto = new PerfilClassificacaoDto();

            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilClassificacaoPorPontuacao(It.IsAny<int>()))
                .ReturnsAsync(classificacaoDto);

            _mapperMock
                .Setup(m => m.Map<PerfilClassificacao>(classificacaoDto))
                .Returns(new PerfilClassificacao(3, 50, 75, new PerfilRisco("Moderado", "Perfil equilibrado")));

            var service = CreateService();

            // Act
            var result = await service.ObterPorClienteId(clienteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Moderado", result.Perfil);
            Assert.Equal("Perfil equilibrado", result.Descricao);
        }

        [Fact(DisplayName = "Não deve retornar perfil se não tiver perfil pontuação parametrizado")]
        public async Task ObterPorClienteId_SemPerfilClassificacao_DeveRetornarMensagem()
        {
            // Arrange
            int clienteId = 10;

            var tipoProduto = new TipoProduto("Tesouro Direto", 1, "Diária", "Tesouro Direto");
            var produto = new Produto(1, "Tesouro Direto", 0.1m, 1, tipoProduto);
            var investimentos = new List<Investimento>
                {
                    new Investimento(1, 1, 1, 1000, 0.12m, DateTime.UtcNow, null, produto)
                };

            _investimentoRepositoryMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(investimentos);

            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoVolume(It.IsAny<decimal>()))
                .ReturnsAsync(new PerfilPontuacaoVolumeDto
                {
                    MinValor = 0.01m,
                    MaxValor = 5000,
                    Pontos = 10
                });

            // Score frequência
            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoFrequencia(1))
                .ReturnsAsync(new PerfilPontuacaoFrequenciaDto
                {
                    MinQtd = 1,
                    MaxQtd = 3,
                    Pontos = 10
                });

            // Score risco
            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilPontuacaoRiscoPorRiscos(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<PerfilPontuacaoRiscoDto>
                {
                        new PerfilPontuacaoRiscoDto
                        {
                            Id = 1,
                            PontosBase = 30,
                            PontosMaximos = 100,
                            Multiplicador = 1.5m
                        }
                });


            _perfilRiscoRepositoryMock
                .Setup(r => r.ObterPerfilClassificacaoPorPontuacao(It.IsAny<int>()))
                .ReturnsAsync((PerfilClassificacaoDto?)null);

            var service = CreateService();

            // Act
            var result = await service.ObterPorClienteId(clienteId);

            // Assert
            Assert.Null(result);
            Assert.Contains(_notificador.ObterNotificacoes(),
                n => n.Mensagem == Mensagens.NaoFoiPossivelDeterminarPerfilRisco);
        }
    }

    public static class LoggerTestExtensions
    {
        public static void VerifyLogWarning<T>(this Mock<ILogger<T>> loggerMock, string message)
        {
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}