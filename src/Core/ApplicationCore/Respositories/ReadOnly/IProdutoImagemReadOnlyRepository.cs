using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IProdutoImagemReadOnlyRepository : IReadOnlyRepository<ProdutoImagemEntity>
    {
        List<ProdutoImagemEntity> ListarPorIds(IEnumerable<int> idsProdutos);
        List<ProdutoImagemEntity> ListarPorId(int idProduto);
        List<ProdutoImagemEntity> Listar(IEnumerable<int> idsProdutoImagem);
        int RecuperarOrdem(int idProduto);
    }
}
