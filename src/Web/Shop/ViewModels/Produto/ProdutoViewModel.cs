using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using Shop.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shop.ViewModels.Produto
{
    public class ProdutoViewModel
    {
        public ProdutoViewModel()
        { }

        public ProdutoViewModel(ProdutoDataValue model)
        {
            Id = model.Id;
            Titulo = model.Titulo;
            Previa = model.Previa;
            DescricaoCurta = model.DescricaoCurta;
            DescricaoLonga = model.DescricaoLonga;
            PrecoDe = model.PrecoDe;
            PrecoPor = model.PrecoPor;
            ImagemPrincipal = model.Imagem;
            QuantidadeMaximaParcelamento = model.QuantidadeMaximaParcelamento;
        }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? IdProdutoSku { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? Quantidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Titulo { get; set; }

        public string Previa { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public bool Destaque { get; set; }
        public decimal? PrecoDe { get; set; }
        public decimal? PrecoPor { get; set; }
        public string ImagemPrincipal { get; set; }
        public int QuantidadeMaximaParcelamento { get; set; }
        public List<string> Imagens { get; set; } = new List<string>();
        public List<ProdutoSkuViewModel> Skus { get; set; } = new List<ProdutoSkuViewModel>();

        public void FillSkus(List<ProdutoSkuDataValue> skus)
        {
            Skus = skus.Select(c => new ProdutoSkuViewModel(c)).ToList();
        }

        public void FillImagens(List<ProdutoImagemEntity> imagens)
        {
            Imagens = imagens.OrderBy(c => c.Ordem).Select(c => c.Original).ToList();
        }
    }
}
