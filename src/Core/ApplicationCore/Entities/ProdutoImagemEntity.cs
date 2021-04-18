namespace ApplicationCore.Entities
{
    public class ProdutoImagemEntity : EntityBase, IEntityDateLog
    {
        public byte Ordem { get; set; }
        public string Original { get; set; }
        public string Grande { get; set; }
        public string Media { get; set; }
        public string Pequena { get; set; }

        public int IdProduto { get; set; }
        public ProdutoEntity Produto { get; set; }
    }
}
