using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class UsuarioEntity : EntityBase, IEntityDateLog
    {
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Imagem { get; set; }

        public string Senha { get; set; }

        public byte[] Salt { get; set; }

        public EStatus Status { get; set; }

        public int IdGrupoUsuario { get; set; }

        public GrupoUsuarioEntity GrupoUsuario { get; set; }
    }
}
