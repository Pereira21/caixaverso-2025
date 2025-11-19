using FluentAssertions;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class PerfisRiscoEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PerfisRiscoEndpointTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterPerfilRisco_Retorna200QuandoExiste()
        {
            // Arrange
            var clienteId = 123;
            var expectedPerfil = new PerfilRiscoResponse
            {
                ClienteId = clienteId,
                Perfil = "Conservador",
                Descricao = "Perfil de risco conservador"
            };

            var mock = new Mock<IPerfilRiscoService>();
            mock.Setup(s => s.ObterPorClienteId(clienteId))
                .ReturnsAsync(expectedPerfil);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var desc = services.SingleOrDefault(d => d.ServiceType == typeof(IPerfilRiscoService));
                    if (desc != null) services.Remove(desc);
                    services.AddSingleton(mock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync($"/perfil-risco/{clienteId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<PerfilRiscoResponse>();
            (content.ClienteId).Should().Be(clienteId);
            (content.Perfil).Should().Be("Conservador");
        }

        [Fact]
        public async Task ObterProdutosRecomendados_Retorna200ComLista()
        {
            // Arrange
            var perfil = "Moderado";
            var produtos = new List<ProdutoRecomendadoResponse>
            {
                new ProdutoRecomendadoResponse()
                {
                    Id = 1, Nome = "Poupança", Rentabilidade = 0.05m, Risco = "Baixo"
                },
                new ProdutoRecomendadoResponse()
                {
                    Id = 2, Nome = "CDB", Rentabilidade = 0.10m, Risco = "Baixo"
                }
            };

            var mock = new Mock<IPerfilRiscoService>();
            mock.Setup(s => s.ObterProdutosRecomendadosPorPerfil(perfil, 1, 200))
                .ReturnsAsync(produtos.ToList());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var desc = services.SingleOrDefault(d => d.ServiceType == typeof(IPerfilRiscoService));
                    if (desc != null) services.Remove(desc);
                    services.AddSingleton(mock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync($"/produtos-recomendados/{perfil}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lista = await response.Content.ReadFromJsonAsync<List<ProdutoRecomendadoResponse>>();
            lista.Should().NotBeNull();
            lista!.Count.Should().Be(2);
            (lista[0].Id).Should().Be(1);
            (lista[0].Nome).Should().Be("Poupança");
        }
    }
}
