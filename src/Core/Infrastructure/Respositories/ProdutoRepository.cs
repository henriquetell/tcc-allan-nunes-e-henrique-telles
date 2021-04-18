using ApplicationCore.DataValue;
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
            var query = DbContext.Produto.Include(c => c.ProdutoImagem).AsQueryable();

            if (!string.IsNullOrWhiteSpace(consulta))
                query = query.Where(c => c.Titulo.Contains(consulta));

            return Paginar(query, paginador);
        }

        public ProdutoEntity Recuperar(int id, bool includeImagens = false)
        {
            return includeImagens
                ? DbContext.Produto.Include(i => i.ProdutoImagem).FirstOrDefault(c => c.Id == id)
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

        public List<ProdutoDataValue> ListarDestaque()
        {
            return (from p in DbContext.Produto
                    let img = p.ProdutoImagem.OrderByDescending(i => i.Ordem).Select(i => i.Original).FirstOrDefault()
                    where
                    p.ProdutoImagem.Any() &&
                    p.Status == EStatus.Ativo
                    select new ProdutoDataValue
                    {
                        Id = p.Id,
                        Titulo = p.Titulo,
                        Codigo = p.Codigo,
                        DescricaoCurta = p.DescricaoCurta,
                        DescricaoLonga = p.DescricaoLonga,
                        Preco = p.Preco,
                        Imagem = img,
                        CategoriaProduto = p.CategoriaProduto
                    }).ToList();
        }

        public List<ProdutoDataValue> ListarParaCatalago(ECategoriaProduto? categoriaProduto = null)
        {
            var produto = categoriaProduto.HasValue
                ? DbContext.Produto.Where(p => p.CategoriaProduto == categoriaProduto.Value && p.Status == EStatus.Ativo)
                : DbContext.Produto.Where(p => p.Status == EStatus.Ativo);

            return (from p in produto
                    let img = p.ProdutoImagem.OrderByDescending(i => i.Ordem).Select(i => i.Original).FirstOrDefault()
                    where p.Status == EStatus.Ativo &&
                           p.ProdutoImagem.Any()
                    select new ProdutoDataValue
                    {
                        Id = p.Id,
                        Titulo = p.Titulo,
                        Codigo = p.Codigo,
                        DescricaoCurta = p.DescricaoCurta,
                        DescricaoLonga = p.DescricaoLonga,
                        Preco = p.Preco,
                        Imagem = img,
                        CategoriaProduto = p.CategoriaProduto,
                    }).ToList();
        }

        public ProdutoDataValue RecuperarParaDetalhes(int idProduto)
        {
            return DbContext.Produto.Where(p=> p.Id == idProduto &&
                    p.Status == EStatus.Ativo &&
                    p.ProdutoImagem.Any())
                .Select(p=> new ProdutoDataValue
                    {
                        Id = p.Id,
                        Titulo = p.Titulo,
                        Codigo = p.Codigo,
                        DescricaoCurta = p.DescricaoCurta,
                        DescricaoLonga = p.DescricaoLonga,
                        Preco = p.Preco,
                        CategoriaProduto = p.CategoriaProduto
                    }).FirstOrDefault();
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
