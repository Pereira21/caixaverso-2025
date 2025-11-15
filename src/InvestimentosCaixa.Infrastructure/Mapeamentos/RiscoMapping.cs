using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class RiscoMapping : IEntityTypeConfiguration<Risco>
    {
        public void Configure(EntityTypeBuilder<Risco> builder)
        {
            builder.ToTable("Risco");

            builder.HasKey(tp => tp.Id);

            builder.Property(tp => tp.Id)
                .ValueGeneratedOnAdd();

            builder.Property(tp => tp.Nome)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(tp => tp.Descricao)
                .HasColumnType("VARCHAR(100)")
                .IsRequired(false);
        }
    }
}
