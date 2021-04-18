using ApplicationCore.Entities;
using Framework.Extenders;

namespace Infrastructure.Respositories.Mappings
{
    internal class ProdutoImagemEntityMap : EntityMapConfiguration<ProdutoImagemEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Original)
                .HasMaxLength(80)
                .IsRequired();

            Builder.Property(t => t.Grande)
                .HasMaxLength(80);

            Builder.Property(t => t.Media)
                .HasMaxLength(80);

            Builder.Property(t => t.Pequena)
                .HasMaxLength(80);

            Builder.HasOneWithMany(t => t.Produto);
        }

    }
}
