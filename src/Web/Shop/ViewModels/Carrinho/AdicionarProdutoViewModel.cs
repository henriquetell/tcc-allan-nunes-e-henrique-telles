using Shop.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels.Carrinho
{
    public class AdicionarProdutoViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? IdProdutoSku { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? Quantidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Titulo { get; set; }
    }
}
