using AutoMapper;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using InvestimentosCaixa.Application.Services;
using InvestimentosCaixa.Domain.Entidades;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Tests.Unitarios
{
    public class LogTelemetriaServiceTests
    {
        private readonly Mock<ILogger<ILogTelemetriaService>> _loggerMock;
        private readonly Mock<INotificador> _notificadorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogTelemetriaRepository> _repoMock;

        private readonly LogTelemetriaService _service;

        public LogTelemetriaServiceTests()
        {
            _loggerMock = new Mock<ILogger<ILogTelemetriaService>>();
            _notificadorMock = new Mock<INotificador>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<ILogTelemetriaRepository>();

            _service = new LogTelemetriaService(
                _loggerMock.Object,
                _notificadorMock.Object,
                _mapperMock.Object,
                _uowMock.Object,
                _repoMock.Object
            );
        }

        /// <summary>
        /// Nesse método de consulta, o log sempre deve armazenar o userId, email e o clienteId consultado.
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Deve registrar log correto quando obtem período mensal")]
        public async Task ObterPeriodoMensalAsync_DeveRegistrarLogCorreto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "user@email.com";

            _repoMock.Setup(r => r.ObterTelemetriaMensalAsync())
                     .ReturnsAsync(new List<TelemetriaResponse>());

            // Act
            await _service.ObterPeriodoMensalAsync(userId, email);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((obj, _) =>
                        obj.ToString()!.Contains(userId.ToString()) &&
                        obj.ToString()!.Contains(email)
                    ),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        [Fact(DisplayName = "Deve chamar repositório quando obtem periodo mensal")]
        public async Task ObterPeriodoMensalAsync_DeveChamarRepositorio()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "teste@teste.com";

            var retornoEsperado = new List<TelemetriaResponse>();

            _repoMock.Setup(r => r.ObterTelemetriaMensalAsync())
                     .ReturnsAsync(retornoEsperado);

            // Act
            var result = await _service.ObterPeriodoMensalAsync(userId, email);

            // Assert
            _repoMock.Verify(r => r.ObterTelemetriaMensalAsync(), Times.Once);
            Assert.Equal(retornoEsperado, result);
        }

        [Fact(DisplayName = "Deve retornar lista vazia se repositorio retornar vazio")]
        public async Task ObterPeriodoMensalAsync_DeveRetornarListaVaziaSeRepositorioRetornarVazio()
        {
            // Arrange
            _repoMock.Setup(r => r.ObterTelemetriaMensalAsync())
                     .ReturnsAsync(new List<TelemetriaResponse>());

            // Act
            var result = await _service.ObterPeriodoMensalAsync(Guid.NewGuid(), "teste@teste.com");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Deve adicionar log e salvar quando adicionar novo")]
        public async Task AdicionarAsync_DeveAdicionarLogEChamarSaveChanges()
        {
            // Arrange
            var req = new LogTelemetriaRequest
            {
                Endpoint = "/api/teste",
                Metodo = "GET",
                TempoRespostaMs = 150,
                Sucesso = true,
                DataRegistro = DateTime.UtcNow
            };

            // Act
            await _service.AdicionarAsync(req);

            // Assert
            _repoMock.Verify(r =>
                r.AdicionarAsync(It.Is<LogTelemetria>(l =>
                       l.Endpoint == req.Endpoint &&
                       l.Metodo == req.Metodo &&
                       l.TempoRespostaMs == req.TempoRespostaMs &&
                       l.Sucesso == req.Sucesso &&
                       l.DataRegistro == req.DataRegistro
                )),
                Times.Once
            );

            _uowMock.Verify(u => u.SaveChangesAsync(new CancellationToken()), Times.Once);
        }
    }
}
