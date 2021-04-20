using ApplicationCore.DataValue;
using ApplicationCore.DataValue.Common;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using Framework.Data;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IProdutoReadOnlyRepository : IReadOnlyRepository<ProdutoEntity>
    {
        List<ProdutoEntity> ListarPorIds(IEnumerable<int> idsProdutos);
        List<ProdutoEntity> Listar(string consulta, PaginadorInfo paginador);
        ProdutoEntity Recuperar(int id, bool includeImagens = false);
        List<string> ListarNomeInterno(int[] idProduto);
        List<SelectListItemDataValue<EStatus>> ListarParaSelect();
    }
}
