using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class PerfilPontuacaoFrequenciaMapping : IEntityTypeConfiguration<PerfilPontuacaoFrequencia>
    {
        public void Configure(EntityTypeBuilder<PerfilPontuacaoFrequencia> builder)
        {
            builder.ToTable("PerfilPontuacaoFrequencia");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.MinQtd)
                .IsRequired();

            builder.Property(p => p.MaxQtd)
                .IsRequired();

            builder.Property(p => p.Pontos)
                .IsRequired();
        }
    }
}
