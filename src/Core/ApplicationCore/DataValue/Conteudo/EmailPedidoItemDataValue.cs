using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DataValue.Conteudo
{
    public class EmailPedidoItemDataValue
    {
        public int Quantidade { get; set; }
        public int? QuantidadeEstorno { get; set; }
        public decimal PrecoDe { get; set; }
        public decimal PrecoPor { get; set; }
        public decimal? ValorTaxa { get; set; }
        public decimal? ValorFrete { get; set; }
        public string DescricaoProduto { get; set; }
        public string DescricaoSku { get; set; }
    }
}
