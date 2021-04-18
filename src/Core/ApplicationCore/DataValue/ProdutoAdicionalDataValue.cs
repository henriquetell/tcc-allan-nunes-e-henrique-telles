using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Enuns;

namespace ApplicationCore.DataValue
{
    public class ProdutoAdicionalDataValue
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public string NomeExterno { get; set; }
        public string NomeInterno { get; set; }
        public EStatus Status { get; set; }
    }
}
