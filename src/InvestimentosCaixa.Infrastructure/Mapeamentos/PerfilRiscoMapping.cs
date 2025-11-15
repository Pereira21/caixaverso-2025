using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class PerfilRiscoMapping : IEntityTypeConfiguration<PerfilRisco>
    {
        public void Configure(EntityTypeBuilder<PerfilRisco> builder)
        {
            builder.ToTable("PerfilRisco");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
