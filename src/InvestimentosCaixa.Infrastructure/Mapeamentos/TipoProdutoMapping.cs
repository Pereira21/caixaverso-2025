using InvestimentosCaixa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestimentosCaixa.Infrastructure.Mapeamentos
{
    public class TipoProdutoMapping : IEntityTypeConfiguration<TipoProduto>
    {
        public void Configure(EntityTypeBuilder<TipoProduto> builder)
        {
            builder.ToTable("TipoProduto");

            builder.HasKey(tp => tp.Id);

            builder.Property(tp => tp.Id)
                .ValueGeneratedOnAdd();

            builder.Property(tp => tp.Nome)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            builder.Property(tp => tp.RiscoId)
                .IsRequired();

            builder.Property(tp => tp.Liquidez)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(tp => tp.Descricao)
                .HasColumnType("VARCHAR(200)")
                .IsRequired(false);

            builder.HasMany(tp => tp.Produtos)
                .WithOne(tp => tp.TipoProduto)
                .HasForeignKey(tp => tp.TipoProdutoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(tp => tp.Risco)
                .WithMany()
                .HasForeignKey(tp => tp.RiscoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
