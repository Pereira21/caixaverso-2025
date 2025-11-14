using AutoMapper;
using InvestimentosCaixa.Application.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InvestimentosCaixa.Api.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        protected readonly INotificador _notificador;
        protected readonly IMapper _mapper;

        public MainController(IMapper mapper, INotificador notificador)
        {
            _mapper = mapper;
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida()) return Ok(result);

            return BadRequest(_notificador.ObterNotificacoes().Select(n => n.Mensagem));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);

            return CustomResponse();
        }

        private void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string errorMsg)
        {
            _notificador.Handle(new Notificacao(errorMsg));
        }
    }
}
