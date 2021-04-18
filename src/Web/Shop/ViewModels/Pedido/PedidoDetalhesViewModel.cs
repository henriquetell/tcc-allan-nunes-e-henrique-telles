using ApplicationCore.DataValue;
using ApplicationCore.Enuns;
using System;
using System.Collections.Generic;

namespace Shop.ViewModels.Pedido
{
    public class PedidoDetalhesViewModel
    {
        public PedidoDetalhesViewModel() { }

        public PedidoDetalhesViewModel(PedidoDataValue pedido)
        {
            Id = pedido.Id;
            DataCriacao = pedido.DataCriacao;
            StatusPedido = pedido.StatusPedido;
            ValorTotal = pedido.ValorTotal;
            ValorTotalFrete = pedido.ValorTotalFrete;
            ValorTotalItens = pedido.ValorTotalItens;
            ValorTotalTaxas = pedido.ValorTotalTaxas;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdCliente { get; set; }
        public decimal ValorTotalItens { get; set; }
        public string Documento { get; set; }
        public EStatusPedido StatusPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal? ValorTotalTaxas { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? ValorTotalFrete { get; set; }

        public IEnumerable<PedidoItemDetalhesViewModel> Itens { get; set; } = new List<PedidoItemDetalhesViewModel>();
    }
}
