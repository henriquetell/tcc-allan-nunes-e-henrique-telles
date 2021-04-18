using System.ComponentModel.DataAnnotations;
using Admin.Resources;

namespace Admin.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Senha { get; set; }

        public bool LembrarMe { get; set; }
    }
}
