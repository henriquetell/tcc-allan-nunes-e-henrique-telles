using Microsoft.AspNetCore.Http;

namespace Admin.ViewModels.Produto
{
    public class ProdutoImagemViewModel
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public string Grande { get; set; }
        public string Media { get; set; }
        public string Pequena { get; set; }
        public byte Ordem { get; set; }

        public IFormFile Arquivo { get; set; }

        public string ContentType { get; set; }

        public string Extensao { get; set; }
        public string Original { get; set; }
    }
}
