namespace InvestimentosCaixa.Domain.Entidades
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
            
        }
        protected BaseEntity(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
