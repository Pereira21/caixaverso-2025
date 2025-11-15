namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilPontuacaoFrequencia : BaseEntity
    {
        public PerfilPontuacaoFrequencia() { }

        public int MinQtd { get; private set; }
        public int MaxQtd { get; private set; }
        public int Pontos { get; private set; }
    }
}
