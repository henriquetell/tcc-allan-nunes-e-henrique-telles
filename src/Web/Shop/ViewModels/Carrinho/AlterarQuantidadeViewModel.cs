using Shop.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels.Carrinho
{
    public class AlterarQuantidadeViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? IdCarrinhoItem { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? IdCarrinho { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.RangeInteiroInvalido))]
        public int? IdProdutoSku { get; set; }
    }
}
