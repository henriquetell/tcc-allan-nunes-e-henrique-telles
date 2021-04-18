using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Enuns;

namespace ApplicationCore.DataValue.Conteudo
{
    public class EmailPedidoPagamentoDataValue
    {
        public DateTime? DataEstorno { get; set; }
        public int QuantidadeParcela { get; set; }
        public decimal? ValorEstorno { get; set; }
        public decimal ValorPagamento { get; set; }
        public decimal ValorParcela { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Link { get; set; }
    }
}
