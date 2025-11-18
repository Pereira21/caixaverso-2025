using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Services;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Controllers
{
    public class PerfisRiscoController : MainController
    {
        private readonly IPerfilRiscoService _perfilRiscoService;
        
        public PerfisRiscoController(IMapper mapper, INotificador notificador, IPerfilRiscoService perfilRiscoService) : base (mapper, notificador)
        {
            _perfilRiscoService = perfilRiscoService;
        }

        /// <summary>
        /// Obter Perfil de Risco do Cliente
        /// </summary>
        /// <param name="clienteId">ID do cliente</param>
        /// <response code="200">Perfil encontrado com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="404">O registro solicitado não existe na base de dados</response>
        [HttpGet("perfil-risco/{clienteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPerfilRisco(int clienteId)
        {
            var perfilRisco = await _perfilRiscoService.ObterPorClienteId(clienteId);
            return CustomResponse(perfilRisco);
        }

        /// <summary>
        /// Obter Produtos Recomendados para um Perfil específico
        /// </summary>
        /// <param name="perfil">Perfil de Risco</param>
        /// <response code="200">Produtos retornados com sucesso</response>
        /// <response code="400">Não foi possível processar a requisição devido a parâmetros inválidos</response>
        /// <response code="401">Acesso não autorizado. Verifique suas credenciais ou o token JWT</response>
        /// <response code="404">O registro solicitado não existe na base de dados</response>
        [HttpGet("produtos-recomendados/{perfil}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterProdutosRecomendadosPorPerfil(string perfil)
        {
            var produtosRecomendados = await _perfilRiscoService.ObterProdutosRecomendadosPorPerfil(perfil);
            return CustomResponse(produtosRecomendados);
        }
    }
}
