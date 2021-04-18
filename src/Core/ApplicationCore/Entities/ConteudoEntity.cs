using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ConteudoEntity : EntityBase, IEntityDateLog
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Assunto { get; set; }
        public string DescricaoComplemento { get; set; }
        public int IdConteudo { get; set; }
        public EConteudoChave ConteudoChave => (EConteudoChave) IdConteudo;

        public EStatus Status { get; set; }

        public ICollection<ConteudoAnexoEntity> ConteudoAnexo { get; set; } = new List<ConteudoAnexoEntity>();
    }
}
