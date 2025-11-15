using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class PerfilClassificacaoMapping : IEntityTypeConfiguration<PerfilClassificacao>
    {
        public void Configure(EntityTypeBuilder<PerfilClassificacao> builder)
        {
            builder.ToTable("PerfilClassificacao");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PerfilRiscoId)
                .IsRequired();

            builder.Property(p => p.MinPontuacao)
                .IsRequired();

            builder.Property(p => p.MaxPontuacao)
                .IsRequired();

            builder.HasOne(x => x.PerfilRisco)
                .WithMany()
                .HasForeignKey(x => x.PerfilRiscoId);
        }
    }
}
