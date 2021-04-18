using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories.Mappings
{
    internal class ProdutoSkuEntityMap : EntityMapConfiguration<ProdutoSkuEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            Builder.HasOneWithMany(t => t.Produto);
        }
    }
}
