using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ProdutoEntity : EntityBase, IEntityDateLog
    {
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public decimal Preco { get; set; }
        public ECategoriaProduto CategoriaProduto { get; set; }
        public EStatus Status { get; set; }
        public ICollection<ProdutoImagemEntity> ProdutoImagem { get; set; } = new List<ProdutoImagemEntity>();
        public ICollection<ProdutoSkuEntity> ProdutoSku { get; set; } = new List<ProdutoSkuEntity>();
    }
}
