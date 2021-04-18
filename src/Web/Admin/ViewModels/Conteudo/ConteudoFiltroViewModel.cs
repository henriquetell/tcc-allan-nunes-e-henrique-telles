using Admin.Resources;
using ApplicationCore.Enuns;
using Framework.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Conteudo
{
    public class ConteudoFiltroViewModel
    {
        public int? IdConteudoChave { get; set; }
        public EConteudoChave ConteudoChave => IdConteudoChave.HasValue ? (EConteudoChave)IdConteudoChave : null;
        public string Descricao { get; set; }
        public PaginadorInfo Paginador { get; set; }
        public List<ConteudoViewModel> Itens { get; set; } = new List<ConteudoViewModel>();
    }
}
