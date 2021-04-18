using Admin.Resources;
using ApplicationCore.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Usuario
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Email { get; set; }

        public string Imagem { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int IdGrupoUsuario { get; set; }

        public int Id { get; set; }
    }
}
