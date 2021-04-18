using ApplicationCore.Entities;
using System;

namespace Infrastructure.Respositories.Mappings
{
    internal class PermissaoEntityMap : EntityMapConfiguration<PermissaoEntity, Guid>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.Descricao)
                   .IsRequired()
                   .HasMaxLength(250);

            Builder.Property(t => t.NomeGrupo)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}
