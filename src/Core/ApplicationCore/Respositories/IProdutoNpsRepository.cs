using System;
using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IProdutoNpsRepository : IProdutoNpsReadOnlyRepository, IRepository<ProdutoNpsEntity, Guid>
    {
        NpsDataValue RecuperarPorProdutoNps(Guid? id, int? produto);
        int? TotalPorTipo(int idProduto, ENotaNps[] tipos);
    }
}
