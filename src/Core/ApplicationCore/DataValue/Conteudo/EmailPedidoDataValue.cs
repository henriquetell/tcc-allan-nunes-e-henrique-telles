using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ApplicationCore.Enuns;

namespace ApplicationCore.DataValue.Conteudo
{
    public class EmailPedidoDataValue
    {
        public DateTime? EmailCancelamento;

        public string Nome { get; set; }
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public DateTime? EmailConfirmacao { get; set; }
        public DateTime? EmailEstorno { get; set; }
        public DateTime? EmailPagamentoErro { get; set; }
        public DateTime? EmailPagamentoSucesso { get; set; }
        public EmailPedidoPagamentoDataValue Pagamento { get;  set; }
        public string DescricaoComplementoHtml { get; set; }
        public EmailPedidoItemDataValue DescricaoComplementoObject { get; set; }
    }
}
