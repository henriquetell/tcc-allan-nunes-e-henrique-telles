using ApplicationCore.Enuns;
using System;

namespace ApplicationCore.DataValue
{
    public class ProdutoNpsDataValue
    {
        public Guid Id { get; set; }
        public int IdProduto { get; set; }
        public string Email { get; set; }
        public string Comentario { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataLimite { get; set; }
        public DateTime? DataResposta { get; set; }
        public ENotaNps? Nota { get; set; }
    }
}
