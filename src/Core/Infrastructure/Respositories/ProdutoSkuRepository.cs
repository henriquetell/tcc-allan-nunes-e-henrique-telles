using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ProdutoSkuRepository : RespositoryBase<ProdutoSkuEntity>, IProdutoSkuRepository
    {
        public ProdutoSkuRepository(EfContext dbContext) : base(dbContext) { }

        public List<ProdutoSkuDataValue> ListarParaFiltro(int idProduto)
        {
            return DbContext.ProdutoSku.Where(sku => sku.IdProduto == idProduto)
                .Select(sku =>
                    new ProdutoSkuDataValue
                    {
                        Id = sku.Id,
                        Descricao = sku.Descricao,
                        Titulo = sku.Produto.Titulo,
                        Status = sku.Status
                    }).ToList();
        }

        public List<ProdutoSkuDataValue> Listar(int idProduto)
        {
            return DbContext.ProdutoSku.Where(sku => sku.IdProduto == idProduto)
                .Select(sku =>
                     new ProdutoSkuDataValue
                    {
                        Id = sku.Id,
                        IdProduto = sku.IdProduto,
                        TipoSku = sku.TipoSku,
                        Descricao = sku.Descricao,
                        Status = sku.Status
                    }).ToList();
        }

        public List<ProdutoSkuDataValue> ListarComEstoque(
            int idProduto,
            bool somenteComEstoque,
            params int[] idProdutoSku)
        {
            var query = DbContext.ProdutoSku.Where(sku => sku.IdProduto == idProduto
                        && sku.Status == EStatus.Ativo)
                .Select(sku =>
                         new ProdutoSkuDataValue
                         {
                             Id = sku.Id,
                             IdProduto = sku.IdProduto,
                             TipoSku = sku.TipoSku,
                             Descricao = sku.Descricao,
                             Status = sku.Status
                         });


            if (idProdutoSku.Any())
                query = query.Where(x => idProdutoSku.Contains(x.Id));

            return query.ToList();
        }

        public ProdutoSkuDataValue Recuperar(int idProduto, int idProdutoSku)
        {
            var query = DbContext.ProdutoSku.Where(sku => sku.IdProduto == idProduto &&
                             sku.Id == idProdutoSku &&
                             sku.Status == EStatus.Ativo)
                .Select(sku=>
                         new ProdutoSkuDataValue
                        {
                            Id = sku.Id,
                            IdProduto = sku.IdProduto
                        });

            return query.FirstOrDefault();
        }

        public AdicionarProdutoSkuDataValue RecuperarParaAdicionar(int idProdutoSku)
        {
            var query = DbContext.ProdutoSku.Where(sku => sku.Id == idProdutoSku &&
                              sku.Status == EStatus.Ativo &&
                              sku.Produto.Status == EStatus.Ativo)
                .Select(sku => new AdicionarProdutoSkuDataValue
                {
                    Id = sku.Id,
                    IdProduto = sku.IdProduto,
                    TipoSku = sku.TipoSku,
                    Descricao = sku.Descricao,
                    PrecoDe = sku.Produto.Preco,
                });

            return query.FirstOrDefault(p => p.Saldo > 0);
        }
    }
}
