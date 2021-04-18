using ApplicationCore.Entities;

namespace Infrastructure.Respositories.Mappings
{
    internal class GrupoUsuarioPermisaoAcaoEntityMap : EntityMapConfiguration<GrupoUsuarioPermisaoAcaoEntity>
    {
        protected override void OnMapping()
        {
            Builder.Property(t => t.IdPermissaoAcao)
                .IsRequired();

            Builder.HasOneWithMany(t => t.GrupoUsuario);

            Builder.HasOneWithMany(t => t.PermissaoAcao);
        }
    }
}
