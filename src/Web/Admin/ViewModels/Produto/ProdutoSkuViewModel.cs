using Admin.Resources;
using ApplicationCore.DataValue;
using ApplicationCore.Enuns;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Produto
{
    public class ProdutoSkuViewModel
    {
        public ProdutoSkuViewModel()
        { }

        public ProdutoSkuViewModel(ProdutoSkuDataValue model)
        {
            IdProdutoSku = model.Id;
            IdProduto = model.IdProduto;
            Descricao = model.Descricao;
            Status = model.Status;
            TipoSku = model.TipoSku;
            Saldo = model.Saldo;
        }

        public int IdProdutoSku { get; set; }
        public int IdProduto { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public ETipoSku TipoSku { get; set; }

        public bool MovimentarEstoque { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Lancamento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? Quantidade { get; set; }

        public int Saldo { get; set; }
    }
}
