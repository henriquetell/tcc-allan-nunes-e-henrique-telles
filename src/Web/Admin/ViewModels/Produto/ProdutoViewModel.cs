using Admin.Resources;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Admin.ViewModels.Produto
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public bool Destaque { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public decimal? Preco { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus? Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public ECategoriaProduto? CategoriaProduto { get; set; }

        public string ImagemPrincipal { get; set; }

        public List<ProdutoSkuViewModel> Skus { get; set; } = new List<ProdutoSkuViewModel>();
        public List<ProdutoImagemViewModel> Imagens { get; set; } = new List<ProdutoImagemViewModel>();

        public void Fill(ProdutoEntity model)
        {
            if (model == null)
                return;

            Id = model.Id;
            Titulo = model.Titulo;
            Codigo = model.Codigo;
            DescricaoCurta = model.DescricaoCurta;
            DescricaoLonga = model.DescricaoLonga;
            Preco = model.Preco;
            Status = model.Status;
            CategoriaProduto = model.CategoriaProduto;

            Imagens = model.ProdutoImagem.Select(c => new ProdutoImagemViewModel
            {
                Id = c.Id,
                IdProduto = c.IdProduto,
                Grande = c.Grande,
                Media = c.Media,
                Pequena = c.Pequena,
                Ordem = c.Ordem,
                Original = c.Original
            }).ToList();
        }
    }
}
