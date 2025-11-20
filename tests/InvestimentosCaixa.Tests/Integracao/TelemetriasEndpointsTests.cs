using FluentAssertions;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class TelemetriasEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TelemetriasEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTelemetria_DeveRetornar200ComDados()
        {
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=tecnico;email=usuario@tecnico.com;id=46ECE551-FEB3-45D7-A800-4980EC840D9B");

            var response = await _client.GetAsync("api/Telemetrias/telemetria");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dados = await response.Content.ReadFromJsonAsync<List<TelemetriaResponse>>();
            dados.Should().NotBeNull();
            dados!.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetTelemetria_ComRoleErrada_DeveRetornar403()
        {
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=cliente;email=cliente@test.com;id=00000000-0000-0000-0000-000000000000");

            var response = await _client.GetAsync("api/Telemetrias/telemetria");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetTelemetria_SemToken_DeveRetornar401()
        {
            // Não adiciona header X-Test-User
            var response = await _client.GetAsync("api/Telemetrias/telemetria");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}