using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class GrupoUsuarioEntity : EntityBase, IEntityDateLog
    {

        public string Nome { get; set; }
        public EStatus Status { get; set; }

        public ICollection<UsuarioEntity> Usuario { get; set; } = new List<UsuarioEntity>();
        public ICollection<GrupoUsuarioPermisaoAcaoEntity> GrupoUsuarioPermisaoAcao { get; set; } = new List<GrupoUsuarioPermisaoAcaoEntity>();
    }
}
