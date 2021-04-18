using Framework.Data;
using System.Collections.Generic;

namespace Admin.ViewModels.Usuario
{
    public class UsuarioFiltroViewModel
    {
        public string Consulta { get; set; }
        public PaginadorInfo Paginador { get; set; }
        public List<UsuarioViewModel> Itens { get; set; } = new List<UsuarioViewModel>();
    }
}
