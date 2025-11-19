using AutoMapper;
using FluentAssertions;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Api.Models.Produto;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Tests.Integracao.Helpers;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Tests.Integracao
{
    public class MassaTesteEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<IProdutoRepository> _mockProdutoRepository;
        private readonly IMapper _mapper;

        public MassaTesteEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            _mockProdutoRepository = new Mock<IProdutoRepository>();

            // Resolver mapper via DI real ou mock (dependendo da sua configuração de testes)
            _mapper = factory.Services.GetService(typeof(IMapper)) as IMapper;
        }

        [Fact]
        public async Task ObterUsuarios_DeveRetornar200ComListaUsuarios()
        {
            // Act
            var response = await _client.GetAsync("obter-usuarios/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<UsuarioModel>>();
            result.Should().NotBeNull();
            result!.Should().HaveCount(2);
            result.Select(u => u.Email).Should().Contain(new[] { "usuario@analista.com", "usuario@tecnico.com" });

            // Verifica endpoints do primeiro usuário
            var analista = result.First(u => u.Email == "usuario@analista.com");
            analista.EndpointList.Should().HaveCount(3);
            analista.EndpointList.Select(e => e.Verbo).Should().OnlyContain(v => v == "GET");
        }

        [Fact]
        public async Task ObterProdutos_DeveRetornar200EListaDeProdutos()
        {
            // Act
            var response = await _client.GetAsync("obter-produtos/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<TipoProdutoDisponivelModel>>();

            result.Should().NotBeNull("o endpoint deve retornar uma lista mapeada");
            result!.Should().NotBeEmpty("deve existir ao menos 1 tipo de produto cadastrado ou seeded");

            // validações básicas
            result.All(tp => tp.Id > 0).Should().BeTrue();
            result.All(tp => !string.IsNullOrWhiteSpace(tp.Nome)).Should().BeTrue();

            // Se tiver produtos internos
            result.Where(tp => tp.Produtos != null).ToList()
                  .ForEach(tp =>
                  {
                      tp.Produtos.Should().NotBeNull();
                      tp.Produtos!.All(p => p.Id > 0).Should().BeTrue();
                  });
        }
    }
}
