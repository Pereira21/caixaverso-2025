using AutoMapper;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Tests.Unitarios
{
    public class InvestimentoServiceTests
    {
        private readonly Mock<INotificador> _notificadorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IInvestimentoRepository> _repoMock;
        private readonly Mock<ILogger<InvestimentoService>> _loggerMock;

        private readonly InvestimentoService _service;

        public InvestimentoServiceTests()
        {
            _notificadorMock = new Mock<INotificador>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IInvestimentoRepository>();
            _loggerMock = new Mock<ILogger<InvestimentoService>>();

            _service = new InvestimentoService(
                _notificadorMock.Object,
                _mapperMock.Object,
                _uowMock.Object,
                _repoMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Nesse método de consulta, o log sempre deve armazenar o userId, email e o clienteId consultado.
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Deve registrar log correto quando obtem por cliente")]
        public async Task ObterPorClienteId_DeveRegistrarLogCorreto()
        {
            // Arrange
            int clienteId = 20;
            Guid userId = Guid.NewGuid();
            string email = "teste@teste.com";

            _repoMock.Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                     .ReturnsAsync(new List<Investimento>());

            _mapperMock.Setup(m => m.Map<List<InvestimentoResponse>>(It.IsAny<List<Investimento>>()))
                       .Returns(new List<InvestimentoResponse>());

            // Act
            await _service.ObterPorClienteId(userId, email, clienteId, 1, 200);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((obj, t) =>
                        obj.ToString()!.Contains(userId.ToString()) &&
                        obj.ToString()!.Contains(email) &&
                        obj.ToString()!.Contains(clienteId.ToString())
                    ),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        [Fact(DisplayName = "Deve chamar repositorio pelo clienteId quando obtem por cliente")]
        public async Task ObterPorClienteId_DeveChamarRepositorioComClienteId()
        {
            // Arrange
            int clienteId = 10;
            Guid userId = Guid.NewGuid();
            string email = "teste@teste.com";

            _repoMock
                .Setup(r => r.ObterComProdutoPorClienteId(clienteId))
                .ReturnsAsync(new List<Investimento>());

            _mapperMock
                .Setup(m => m.Map<List<InvestimentoResponse>>(It.IsAny<List<Investimento>>()))
                .Returns(new List<InvestimentoResponse>());

            // Act
            await _service.ObterPorClienteId(userId, email, clienteId, 1, 200);

            // Assert
            _repoMock.Verify(r => r.ObterPaginadoComProdutoPorClienteId(clienteId, 1, 200), Times.Once);
        }

        [Fact(DisplayName = "Deve mapear investimentos para response quando obtem pelo cliente")]
        public async Task ObterPorClienteId_DeveMapearInvestimentosParaResponse()
        {
            // Arrange
            int clienteId = 1;
            Guid userId = Guid.NewGuid();
            string email = "teste@teste.com";

            var investimentos = new List<Investimento>
            {
                new Investimento(1, clienteId, 1, 1000, 0.12m, DateTime.UtcNow),
                new Investimento(2, clienteId, 2, 3000, 0.06m, DateTime.UtcNow),
                new Investimento(1, clienteId, 3, 500, 0.11m, DateTime.UtcNow)
            };

            var mapped = new List<InvestimentoResponse>
            {
                new InvestimentoResponse()
            };

            _repoMock.Setup(r => r.ObterPaginadoComProdutoPorClienteId(clienteId, 1, 200))
                     .ReturnsAsync(investimentos);

            _mapperMock.Setup(m => m.Map<List<InvestimentoResponse>>(investimentos))
                       .Returns(mapped);

            // Act
            var result = await _service.ObterPorClienteId(userId, email, clienteId,1 , 200);

            // Assert
            Assert.Equal(mapped, result);
            _mapperMock.Verify(m => m.Map<List<InvestimentoResponse>>(investimentos), Times.Once);
        }
    }
}
