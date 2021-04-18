using Shop.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels.Login
{
    public class RegistrarViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Documento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [EmailAddress(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.EmailInvalido))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,12}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.NivelSegurancaSenha))]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Compare(nameof(Senha), ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.ConfimacaoSenha))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,12}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.NivelSegurancaSenha))]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; }
    }
}
