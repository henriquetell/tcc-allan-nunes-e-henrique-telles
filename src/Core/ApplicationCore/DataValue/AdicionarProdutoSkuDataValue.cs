using ApplicationCore.Enuns;

namespace ApplicationCore.DataValue
{
    public class AdicionarProdutoSkuDataValue
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public ETipoSku TipoSku { get; set; }
        public string Descricao { get; set; }
        public int Saldo { get; set; }
        public EStatus Status { get; set; }
        public decimal PrecoDe { get; set; }
        public decimal PrecoPor { get; set; }
    }
}
