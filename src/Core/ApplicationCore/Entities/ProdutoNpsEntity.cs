using System;

namespace ApplicationCore.Entities
{
    public class ProdutoNpsEntity : EntityBase<Guid>, IEntityDateLog
    {
        public string Email { get; set; }
        public string Comentario { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataLimite { get; set; }

        public int? Nota { get; set; }

        public int IdProduto { get; set; }
        public ProdutoEntity Produto { get; set; }
    }
}
