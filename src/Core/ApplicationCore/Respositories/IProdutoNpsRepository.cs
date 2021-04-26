using System;
using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IProdutoNpsRepository : IProdutoNpsReadOnlyRepository, IRepository<ProdutoNpsEntity, Guid>
    {
        NpsDataValue RecuperarPorProdutoNps(Guid? id, int? produto);
    }
}
