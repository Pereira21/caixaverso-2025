using FluentAssertions;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class InvestimentosEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public InvestimentosEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ObterPorClienteId_DeveRetornar200ComDados()
        {
            // Simula usuário analista
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=analista;email=analista@teste.com;id=11111111-1111-1111-1111-111111111111");

            int clienteId = 1; // id de teste
            var response = await _client.GetAsync($"api/Investimentos/investimentos/{clienteId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var dados = await response.Content.ReadFromJsonAsync<List<InvestimentoResponse>>();
            dados.Should().NotBeNull();
            dados!.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObterPorClienteId_ComRoleErrada_DeveRetornar403()
        {
            // Simula usuário sem permissão
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=cliente;email=cliente@teste.com;id=22222222-2222-2222-2222-222222222222");

            int clienteId = 1;
            var response = await _client.GetAsync($"api/Investimentos/investimentos/{clienteId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ObterPorClienteId_SemToken_DeveRetornar401()
        {
            int clienteId = 1;
            // Não adiciona header X-Test-User
            var response = await _client.GetAsync($"api/Investimentos/investimentos/{clienteId}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ObterPorClienteId_SemInvestimentos_DeveRetornar204()
        {
            // Simula usuário analista
            _client.DefaultRequestHeaders.Add("X-Test-User",
                "role=analista;email=analista@teste.com;id=33333333-3333-3333-3333-333333333333");

            int clienteId = 999; // cliente sem investimentos
            var response = await _client.GetAsync($"api/Investimentos/investimentos/{clienteId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
