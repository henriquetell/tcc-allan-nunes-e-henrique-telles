using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ProdutoImagemRepository : RespositoryBase<ProdutoImagemEntity>, IProdutoImagemRepository
    {
        public ProdutoImagemRepository(EfContext dbContext) : base(dbContext) { }

        public List<ProdutoImagemEntity> ListarPorIds(IEnumerable<int> idsProdutos)
        {
            var ids = idsProdutos.ToList();
            return DbContext.ProdutoImagem.Where(pi => ids.Contains(pi.IdProduto))
                .ToList();
        }

        public List<ProdutoImagemEntity> ListarPorId(int idProduto)
        {
            return DbContext.ProdutoImagem.Where(pi => pi.IdProduto == idProduto)
                .OrderBy(pi => pi.Ordem)
                .ToList();
        }

        public List<ProdutoImagemEntity> Listar(IEnumerable<int> idsProdutoImagem)
        {
            return DbContext.ProdutoImagem.Where(c => idsProdutoImagem.Contains(c.Id)).ToList();
        }

        public int RecuperarOrdem(int idProduto)
        {
            return DbContext.ProdutoImagem.OrderByDescending(c => c.Ordem).FirstOrDefault(c => c.IdProduto == idProduto)?.Ordem ?? 0;
        }
    }
}
