using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ProdutoEntity : EntityBase, IEntityDateLog
    {
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string Imagem { get; set; }
        public string DescricaoLonga { get; set; }
        public int IdConteudo { get; set; }
        public ECategoriaProduto CategoriaProduto { get; set; }
        public EStatus Status { get; set; }
        public ICollection<ProdutoNpsEntity> ProdutoNps { get; set; } = new List<ProdutoNpsEntity>();
        public ConteudoEntity Conteudo { get; set; }
    }
}
