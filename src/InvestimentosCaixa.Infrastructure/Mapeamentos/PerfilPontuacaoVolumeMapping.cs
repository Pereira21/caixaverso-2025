using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class PerfilPontuacaoVolumeMapping : IEntityTypeConfiguration<PerfilPontuacaoVolume>
    {
        public void Configure(EntityTypeBuilder<PerfilPontuacaoVolume> builder)
        {
            builder.ToTable("PerfilPontuacaoVolume");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.MinValor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.MaxValor)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Pontos)
                .IsRequired();
        }
    }
}
