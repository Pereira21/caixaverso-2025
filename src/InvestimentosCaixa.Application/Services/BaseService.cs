using AutoMapper;
using InvestimentosCaixa.Application.Interfaces.Repositorios;
using InvestimentosCaixa.Application.Notificacoes;

namespace InvestimentosCaixa.Application.Services
{
    public class BaseService
    {
        protected readonly INotificador _notificador;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(INotificador notificador, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _notificador = notificador;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
