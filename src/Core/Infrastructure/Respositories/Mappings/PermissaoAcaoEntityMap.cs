using ApplicationCore.Entities;
using System;

namespace Infrastructure.Respositories.Mappings
{
    internal class PermissaoAcaoEntityMap : EntityMapConfiguration<PermissaoAcaoEntity, Guid>
    {
        protected override void OnMapping()
        {
            Builder.HasOneWithMany(t => t.Permissao, Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
