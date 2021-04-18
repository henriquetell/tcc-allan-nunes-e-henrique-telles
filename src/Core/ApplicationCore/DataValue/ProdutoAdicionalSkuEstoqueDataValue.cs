using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DataValue
{
    public class ProdutoSkuEstoqueDataValue
    {
        public int Id { get; set; }
        public int IdProdutoSku { get; set; }
        public int Quantidade { get; set; }
        public int Saldo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
