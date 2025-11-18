namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilPontuacaoVolume : BaseEntity
    {
        public PerfilPontuacaoVolume() { }
        public decimal MinValor { get; private set; }
        public decimal MaxValor { get; private set; }
        public int Pontos { get; private set; }
    }
}
