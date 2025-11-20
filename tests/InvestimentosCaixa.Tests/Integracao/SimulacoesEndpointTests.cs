using FluentAssertions;
using InvestimentosCaixa.Api.Models.Simulacao;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class SimulacoesEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<ISimulacaoService> _mockSimulacaoService;

        public SimulacoesEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            // Configura mock do serviço
            _mockSimulacaoService = new Mock<ISimulacaoService>();
        }

        [Fact]
        public async Task SimularInvestimento_Valido_DeveRetornar200ComResultado()
        {
            var model = new SimularInvestimentoModel
            {
                Valor = 1000,
                PrazoMeses = 12,
                TipoProduto = "CDB"
            };

            var simulacaoEsperada = new SimularInvestimentoResponse
            {
                ResultadoSimulacao = new ResultadoSimulacaoResponse
                {
                    ValorFinal = 1120.0m,
                    PrazoMeses = 12,
                    RentabilidadeEfetiva = 0.12m
                },
                ProdutoValidado = new ProdutoValidadoResponse
                {
                    Nome = "CDB Caixa",
                    Rentabilidade = 0.12m,
                    Tipo = "CDB"
                }
            };

            _mockSimulacaoService.Setup(x => x.SimularInvestimentoAsync(It.IsAny<SimularInvestimentoRequest>()))
                                 .ReturnsAsync(simulacaoEsperada);

            var response = await _client.PostAsJsonAsync("simular-investimento", model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<SimularInvestimentoResponse>();
            result.Should().NotBeNull();
            ((double)result.ResultadoSimulacao.ValorFinal).Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task SimularInvestimento_ModeloInvalido_DeveRetornar400()
        {
            // ValorInicial ausente
            var model = new SimularInvestimentoModel
            {
                PrazoMeses = 12,
                TipoProduto = "CDB"
            };

            var response = await _client.PostAsJsonAsync("simular-investimento", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ObterSimulacoes_SemAutorizacao_DeveRetornar401()
        {
            var response = await _client.GetAsync("simulacoes");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ObterSimulacoes_ComRoleErrada_DeveRetornar403()
        {
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=cliente;email=cliente@test.com;id=00000000-0000-0000-0000-000000000000");

            var response = await _client.GetAsync("simulacoes");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ObterSimulacoes_ComRoleAnalista_DeveRetornar200()
        {
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=analista;email=analista@test.com;id=11111111-1111-1111-1111-111111111111");

            var simulacoesMock = new List<SimulacaoResponseDTO>
            {
                new SimulacaoResponseDTO{ Id = 1, ValorInvestido = 1000, ValorFinal = 1120, DataSimulacao = DateTime.UtcNow }
            };

            _mockSimulacaoService.Setup(x => x.ObterHistoricoAsync(It.IsAny<Guid>(), It.IsAny<string>(), 1, 200))
                                 .ReturnsAsync(simulacoesMock);

            var response = await _client.GetAsync("simulacoes");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<dynamic>>();
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }
    }
}
