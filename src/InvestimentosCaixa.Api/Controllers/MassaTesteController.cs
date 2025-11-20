using AutoMapper;
using InvestimentosCaixa.Api.Models.Auth;
using InvestimentosCaixa.Api.Models.Produto;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
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
                    EndpointList = new List<EndpointModel>()
                    {
                        new EndpointModel() { Url = "/investimentos/{clienteId}", Verbo = "GET" },
                        new EndpointModel() { Url = "/simulacoes", Verbo = "GET" },
                        new EndpointModel() { Url = "/simulacoes/por-produto-dia", Verbo = "GET" }
                    }
                },
                new UsuarioModel()
                {
                    Email = "usuario@tecnico.com",
                    Senha = "@Tecnico123",
                    EndpointList = new List<EndpointModel>()
                    {
                        new EndpointModel() { Url = "telemetria", Verbo = "GET" }
                    }
                }
            };

            return CustomResponse(usuarios);
        }
    }
}
