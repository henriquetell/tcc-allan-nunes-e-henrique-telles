using Admin.Resources;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Login
{
    public class RecuperarSenhaViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Email { get; set; }
    }
}
