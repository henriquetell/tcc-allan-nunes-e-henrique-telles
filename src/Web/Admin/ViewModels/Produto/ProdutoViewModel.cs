using Admin.Resources;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Produto
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public IFormFile Imagem { get; set; }
        public string ImagemUrl { get; set; }
        public string DescricaoLonga { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? IdConteudo { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus? Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public ECategoriaProduto? CategoriaProduto { get; set; }


        public int? TotalDetratores { get; set; }
        public int? TotalPromotores { get; set; }

        public int TotalAvaliacao => ((TotalPromotores ?? 0) - (TotalPromotores ?? 0));

        public void Fill(ProdutoEntity model)
        {
            if (model == null)
                return;

            Id = model.Id;
            IdConteudo = model.IdConteudo;
            Titulo = model.Titulo;
            Codigo = model.Codigo;
            ImagemUrl = model.Imagem;
            DescricaoLonga = model.DescricaoLonga;
            Status = model.Status;
            CategoriaProduto = model.CategoriaProduto;
            TotalPromotores = model.TotalPromotores;
            TotalDetratores = model.TotalDetratores;
        }
    }
}
