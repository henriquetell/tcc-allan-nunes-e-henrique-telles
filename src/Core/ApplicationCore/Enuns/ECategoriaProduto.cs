using System.ComponentModel;

namespace ApplicationCore.Enuns
{
    public enum ECategoriaProduto : int
    {
        [Description("Produto Físico")]
        ProdutoFisico = 1,

        [Description("Produto Virtual")]
        ProdutoVirtual = 2,

        [Description("Prestação e Serviço")]
        Servico = 3,

    }
}
