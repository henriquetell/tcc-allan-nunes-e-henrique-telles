using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ProdutoNpsRepository : RespositoryBase<ProdutoNpsEntity, Guid>, IProdutoNpsRepository
    {
        public ProdutoNpsRepository(EfContext dbContext) : base(dbContext) { }


        public List<ProdutoNpsDataValue> ListarPorProduto(int idProduto)
        {
            return DbContext.ProdutoNps.Where(w => w.IdProduto == idProduto)
                .Select(w => new ProdutoNpsDataValue
                {
                    Id = w.Id,
                    IdProduto = w.IdProduto,
                    Email = w.Email,
                    Comentario = w.Comentario,
                    DataEnvio = w.DataEnvio,
                    DataLimite = w.DataLimite,
                    DataResposta = w.DataResposta,
                    Nota = w.Nota
                })
                .OrderByDescending(w => w.DataEnvio)
                .ToList();
        }

        public NpsDataValue RecuperarPorProdutoNps(Guid? id, int? idProduto)
        {
            return DbContext.ProdutoNps.Where(w => w.IdProduto == idProduto && w.Id == id)
               .Select(w => new NpsDataValue
               {
                   Id = w.Id,
                   Titulo = w.Produto.Titulo,
                   DescricaoLonga = w.Produto.DescricaoLonga,
                   Imagem = w.Produto.Imagem,
                   DataLimite = w.DataLimite,
                   Respondido = w.DataResposta.HasValue
               })
               .FirstOrDefault();
        }

        public int? TotalPorTipo(int idProduto, ENotaNps[] tipos)
        {
            return DbContext.ProdutoNps
                .Where(c => c.IdProduto == idProduto &&
                            c.DataResposta.HasValue &&
                            c.Nota.HasValue &&
                    tipos.Contains(c.Nota.Value))
                .Sum(c => (int)c.Nota.Value);
        }
    }
}
