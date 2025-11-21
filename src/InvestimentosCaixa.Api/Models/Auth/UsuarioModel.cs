namespace InvestimentosCaixa.Api.Models.Auth
{
    public class UsuarioModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Descricao { get; set; }
        public List<EndpointModel> EndpointList { get; set; }
    }

    public class EndpointModel
    {
        public string Url { get; set; }
        public string Verbo { get; set; }
    }
}
