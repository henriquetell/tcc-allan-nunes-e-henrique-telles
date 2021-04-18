using System.ComponentModel.DataAnnotations;
using Shop.Resources;

namespace Shop.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [EmailAddress(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.EmailInvalido))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}
