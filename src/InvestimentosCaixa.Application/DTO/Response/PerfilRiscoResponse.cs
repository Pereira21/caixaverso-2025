namespace InvestimentosCaixa.Application.DTO.Response
{
    public class PerfilRiscoResponse
    {
        public int ClienteId { get; set; }
        public string Perfil { get; set; }
        public int Pontuacao { get; set; }
        public string Descricao { get; set; }
    }
}
