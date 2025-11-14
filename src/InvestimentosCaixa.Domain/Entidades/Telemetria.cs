namespace InvestimentosCaixa.Domain.Entidades
{
    public class Telemetria : BaseEntity
    {
        public string Endpoint { get; private set; }
        public string Metodo { get; private set; }
        public int TempoRespostaMs { get; private set; }
        public bool Sucesso { get; private set; }
        public DateTime DataRegistro { get; private set; }

        public Telemetria(string endpoint, string metodo, int tempoRespostaMs, bool sucesso, DateTime dataRegistro)
        {
            Endpoint = endpoint;
            Metodo = metodo;
            TempoRespostaMs = tempoRespostaMs;
            Sucesso = sucesso;
            DataRegistro = dataRegistro;
        }

        protected Telemetria() { }
    }
}
