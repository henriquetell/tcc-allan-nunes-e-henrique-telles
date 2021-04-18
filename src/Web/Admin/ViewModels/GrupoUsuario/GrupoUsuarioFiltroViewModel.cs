using Framework.Data;
using System.Collections.Generic;

namespace Admin.ViewModels.GrupoUsuario
{
    public class GrupoUsuarioFiltroViewModel
    {
        public string Consulta { get; set; }
        public PaginadorInfo Paginador { get; set; }
        public List<GrupoUsuarioViewModel> Itens { get; set; } = new List<GrupoUsuarioViewModel>();
    }
}
