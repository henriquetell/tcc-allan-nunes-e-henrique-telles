using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class PermissaoEntity : EntityBase<Guid>, IEntityDateLog
    {
        public string Descricao { get; set; }
        public string NomeGrupo { get; set; }

        public ICollection<PermissaoAcaoEntity> PermissaoAcao { get; set; } = new List<PermissaoAcaoEntity>();
    }
}
