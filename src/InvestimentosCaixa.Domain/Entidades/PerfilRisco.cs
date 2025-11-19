namespace InvestimentosCaixa.Domain.Entidades
{
    public class PerfilRisco : BaseEntity
    {
        public PerfilRisco() { }

        public PerfilRisco(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
            RelPerfilRiscoList = new List<RelPerfilRisco>();
        }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        public List<RelPerfilRisco> RelPerfilRiscoList { get; private set; }

        public void AdicionarRisco(int riscoId)
        {
            RelPerfilRiscoList.Add(new RelPerfilRisco(Id, riscoId));
        }
    }
}
