using ApplicationCore.DataValue;
using ApplicationCore.Entities;
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
    }
}
