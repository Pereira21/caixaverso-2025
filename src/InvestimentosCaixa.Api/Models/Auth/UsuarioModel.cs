namespace InvestimentosCaixa.Api.Models.Auth
{
    /// <summary>
    /// Model fora de padrão exclusiva para avaliador testar o sistema
    /// </summary>
    public class UsuarioModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<EndpointModel> EndpointList { get; set; }
    }

    public class EndpointModel
    {
        public string Url { get; set; }
        public string Verbo { get; set; }
    }
}
