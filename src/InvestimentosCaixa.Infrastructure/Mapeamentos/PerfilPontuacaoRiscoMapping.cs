using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class PerfilPontuacaoRiscoMapping : IEntityTypeConfiguration<PerfilPontuacaoRisco>
    {
        public void Configure(EntityTypeBuilder<PerfilPontuacaoRisco> builder)
        {
            builder.ToTable("PerfilPontuacaoRisco");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RiscoId)
                .IsRequired();

            builder.Property(p => p.PontosBase)
                .IsRequired();

            builder.Property(p => p.Multiplicador)
                .HasColumnType("decimal(2,1)")
                .IsRequired();

            builder.Property(p => p.PontosMaximos)
                .IsRequired();

            builder.HasOne(p => p.Risco)
                .WithMany()
                .HasForeignKey(p => p.RiscoId);
        }
    }
}