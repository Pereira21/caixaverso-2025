using AutoMapper;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Api.Models.Produto;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    /// <summary>
    /// Controlador focado em requisições para auxílio no preenchimento de outros endpoints
    /// </summary>
    [Route("api/[controller]")]
    public class MassaTesteController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public MassaTesteController(IMapper mapper, INotificador notificador, IProdutoRepository produtoRepository) : base(mapper, notificador)
        {
            _produtoRepository = produtoRepository;
        }

        /// <summary>
        /// [Não oficial] Obter lista de produtos disponíveis para simulação
        /// </summary>
        /// <response code="200">Produtos obtidos com sucesso</response>
        [HttpGet("obter-produtos/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterProdutos()
        {
            var tipoProdutoList = await _produtoRepository.ObterTipoProdutoComProdutosAsync();
            var response = _mapper.Map<List<TipoProdutoDisponivelModel>>(tipoProdutoList);
            return CustomResponse(response);
        }

        /// <summary>
        /// [Não oficial] Obter lista de usuários para login
        /// </summary>
        /// <response code="200">Usuários obtidos com sucesso</response>
        [HttpGet("obter-usuarios/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterUsuarios()
        {
            var usuarios = new List<UsuarioModel>()
            {
                new UsuarioModel()
                {
                    Email = "usuario@analista.com",
                    Senha = "@Analista123",
                    Descricao = "Usuário de perfil analista. Role 'analista'",
                    EndpointList = new List<EndpointModel>()
                    {
                        new EndpointModel() { Url = "api/Investimentos/investimentos/{clienteId}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/PerfisRisco/perfil-risco/{clienteId}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/PerfisRisco/produtos-recomendados/{perfil}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/Simulacoes/simular-investimento/", Verbo = "POST" },
                        new EndpointModel() { Url = "api/Simulacoes/simulacoes", Verbo = "GET" },
                        new EndpointModel() { Url = "api/Simulacoes/simulacoes/por-produto-dia", Verbo = "GET" }
                    }
                },
                new UsuarioModel()
                {
                    Email = "usuario@tecnico.com",
                    Senha = "@Tecnico123",
                    Descricao = "Usuário de perfil técnico. Role 'tecnico'",
                    EndpointList = new List<EndpointModel>()
                    {
                        new EndpointModel() { Url = "api/PerfisRisco/perfil-risco/{clienteId}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/PerfisRisco/produtos-recomendados/{perfil}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/Simulacoes/simular-investimento/", Verbo = "POST" },
                        new EndpointModel() { Url = "telemetria", Verbo = "GET" }
                    }
                },
                new UsuarioModel()
                {
                    Email = "usuario@usuario.com",
                    Senha = "@Usuario123",
                    Descricao = "Usuário comum sem privilégios",
                    EndpointList = new List<EndpointModel>()
                    {
                        new EndpointModel() { Url = "api/PerfisRisco/perfil-risco/{clienteId}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/PerfisRisco/produtos-recomendados/{perfil}", Verbo = "GET" },
                        new EndpointModel() { Url = "api/Simulacoes/simular-investimento/", Verbo = "POST" },
                    }
                }
            };

            return CustomResponse(usuarios);
        }
    }
}
