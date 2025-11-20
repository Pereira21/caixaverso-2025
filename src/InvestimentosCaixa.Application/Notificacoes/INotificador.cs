namespace InvestimentosCaixa.Application.Notificacoes
{
    public interface INotificador
    {
        /// <summary>
        /// Verifica se há notificações
        /// </summary>
        /// <returns></returns>
        bool TemNotificacao();

        /// <summary>
        /// Obter todas as notificações
        /// </summary>
        /// <returns></returns>
        List<Notificacao> ObterNotificacoes();

        /// <summary>
        /// Adiciona Notificação
        /// </summary>
        /// <param name="notificacao"></param>
        void Handle(Notificacao notificacao);
    }
}
