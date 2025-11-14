namespace InvestimentosCaixa.Domain.Entidades
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
            
        }

        public int Id { get; private set; }
    }
}
