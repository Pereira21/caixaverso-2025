using InvestimentosCaixa.Application.Notificacoes;

namespace InvestimentosCaixa.Application.Services
{
    public class BaseService
    {
        protected readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        //protected void Notificar(ValidationResult validationResult)
        //{
        //    foreach (var item in validationResult.Errors)
        //    {
        //        Notificar(item.ErrorMessage);
        //    }
        //}

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
