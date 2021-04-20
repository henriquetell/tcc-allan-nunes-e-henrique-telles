using ApplicationCore.DataValue.Common;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories;
using Framework.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ProdutoRepository : RespositoryBase<ProdutoEntity>, IProdutoRepository
    {
        public ProdutoRepository(EfContext dbContext) : base(dbContext) { }

        public List<ProdutoEntity> ListarPorIds(IEnumerable<int> idsProdutos)
        {
            var ids = idsProdutos.ToArray();
            return DbContext.Produto.Where(p => ids.Contains(p.Id) &&
                                                p.Status == EStatus.Ativo)
                .ToList();
        }

        public List<ProdutoEntity> Listar(string consulta, PaginadorInfo paginador)
        {
            var query = DbContext.Produto.Include(c => c.ProdutoNps).AsQueryable();

            if (!string.IsNullOrWhiteSpace(consulta))
                query = query.Where(c => c.Titulo.Contains(consulta));

            return Paginar(query, paginador);
        }

        public ProdutoEntity Recuperar(int id, bool includeImagens = false)
        {
            return includeImagens
                ? DbContext.Produto.Include(i => i.ProdutoNps).FirstOrDefault(c => c.Id == id)
                : DbContext.Produto.FirstOrDefault(c => c.Id == id);
        }

        public List<ProdutoEntity> ListarProdutosPorCampanha(int idCampanha)
        {
            return DbContext.Produto
                .OrderBy(c => c.Titulo)
                .ToList();
        }

        public List<string> ListarNomeInterno(int[] idProduto)
        {
            return DbContext.Produto
                .Where(c => idProduto.Contains(c.Id))
                .Select(c => c.Titulo)
                .ToList();
        }


        public List<SelectListItemDataValue<EStatus>> ListarParaSelect()
        {
            return DbContext.Produto.Select(c => new SelectListItemDataValue<EStatus>
            {
                Value = c.Id.ToString(),
                Text = c.Titulo,
                Enum = c.Status
            })
                .ToList();
        }
    }
}
