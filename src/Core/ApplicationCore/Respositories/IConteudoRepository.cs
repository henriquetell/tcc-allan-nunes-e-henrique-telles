using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IConteudoRepository : IConteudoReadOnlyRepository, IRepository<ConteudoEntity>
    {
    }
}
