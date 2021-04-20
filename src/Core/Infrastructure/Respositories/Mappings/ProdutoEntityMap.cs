using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories.Mappings
{
    internal class ProdutoEntityMap : EntityMapConfiguration<ProdutoEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Titulo)
                .HasMaxLength(1000)
                .IsRequired();

            Builder.Property(t => t.Codigo)
                .HasMaxLength(2000)
                .IsRequired(true);

            Builder.Property(t => t.Imagem)
                .HasMaxLength(3000)
                .IsRequired(false);

            Builder.Property(t => t.DescricaoLonga)
                .HasColumnType("varchar(max)")
                .IsRequired(false);

            Builder.HasIndex(t => t.Codigo)
                .IsUnique();

            Builder.HasOneWithMany(t => t.Conteudo);
        }
    }
}
