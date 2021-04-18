using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IConteudoAnexoReadOnlyRepository : IReadOnlyRepository<ConteudoAnexoEntity>
    {
        List<ConteudoAnexoEntity> Listar(int id);
    }
}
