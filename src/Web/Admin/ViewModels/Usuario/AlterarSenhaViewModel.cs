using Admin.Resources;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Usuario
{
    public class AlterarSenhaViewModel
    {
        public AlterarSenhaViewModel()
        { }

        public AlterarSenhaViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [MinLength(6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "TamanhoMinimoDaSenha")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,120}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "ValidacaoSegurancaDaSenha", ErrorMessage = null)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [MinLength(6, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "TamanhoMinimoDaSenha")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,120}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "ValidacaoSegurancaDaSenha", ErrorMessage = null)]
        [Compare(nameof(NovaSenha), ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "ConfimacaoSenha")]
        public string ConfirmarNovaSenha { get; set; }

        public string ReturnUrl { get; set; }
    }
}
