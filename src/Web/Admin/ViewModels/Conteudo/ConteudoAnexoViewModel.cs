using Microsoft.AspNetCore.Http;

namespace Admin.ViewModels.Conteudo
{
    public class ConteudoAnexoViewModel
    {
        public int Id { get; set; }
        public int IdConteudo { get; set; }
        public string Anexo { get; set; }
        public IFormFile Arquivo { get; set; }
        public string NomeOriginal { get; set; }
    }
}
