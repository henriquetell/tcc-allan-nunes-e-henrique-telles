using System;

namespace ApplicationCore.Entities
{
    public class GrupoUsuarioPermisaoAcaoEntity : EntityBase
    {
        public int IdGrupoUsuario { get; set; }
        public GrupoUsuarioEntity GrupoUsuario { get; set; }

        public Guid IdPermissaoAcao { get; set; }
        public PermissaoAcaoEntity PermissaoAcao {get;set;}
    }
}
