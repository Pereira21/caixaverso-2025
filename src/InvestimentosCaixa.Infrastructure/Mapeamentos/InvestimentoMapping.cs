using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class InvestimentoMapping : IEntityTypeConfiguration<Investimento>
    {
        public void Configure(EntityTypeBuilder<Investimento> builder)
        {
            builder.ToTable("Investimento");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Valor)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Rentabilidade)
                   .HasColumnType("decimal(5,4)")
                   .IsRequired();

            builder.Property(i => i.Data)
                   .IsRequired();

            builder.HasOne(i => i.Cliente)
                   .WithMany()
                   .HasForeignKey(i => i.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Produto)
                   .WithMany()
                   .HasForeignKey(i => i.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
