using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IConteudoAnexoRepository : IConteudoAnexoReadOnlyRepository, IRepository<ConteudoAnexoEntity>
    {
    }
}
