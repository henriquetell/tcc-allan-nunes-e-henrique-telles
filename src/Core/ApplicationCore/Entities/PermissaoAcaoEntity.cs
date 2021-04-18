using Framework.Security.Authorization;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class PermissaoAcaoEntity : EntityBase<Guid>, IEntityDateLog
    {
        public int TipoAcao { get; set; }

        public AuthPermissaoTipoAcao AuthAcao => (AuthPermissaoTipoAcao)TipoAcao;

        public Guid IdPermissao { get; set; }
        public PermissaoEntity Permissao { get; set; }

        public ICollection<GrupoUsuarioPermisaoAcaoEntity> GrupoUsuarioPermisaoAcao { get; set; } = new List<GrupoUsuarioPermisaoAcaoEntity>();
    }
}
