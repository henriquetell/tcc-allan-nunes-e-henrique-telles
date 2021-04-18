using ApplicationCore.Enuns;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ProdutoSkuEntity : EntityBase, IEntityDateLog
    {
        public string Descricao { get; set; }
        public EStatus Status { get; set; }
        public ETipoSku TipoSku { get; set; }
        public int IdProduto { get; set; }
        public ProdutoEntity Produto { get; set; }

    }
}
