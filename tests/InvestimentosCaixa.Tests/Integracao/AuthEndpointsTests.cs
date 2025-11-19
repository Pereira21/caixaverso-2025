using FluentAssertions;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class AuthEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_Valido_DeveRetornar200ComToken()
        {
            var model = new LoginModel
            {
                Email = "usuario@teste.com",
                Senha = "SenhaForte123!"
            };

            var response = await _client.PostAsJsonAsync("login", model);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            content.Should().NotBeNull();
            content!.Should().ContainKey("token");
            content["token"].Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_ModeloInvalido_DeveRetornar400()
        {
            // Senha ausente
            var model = new LoginModel
            {
                Email = ""
            };

            var response = await _client.PostAsJsonAsync("login", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
