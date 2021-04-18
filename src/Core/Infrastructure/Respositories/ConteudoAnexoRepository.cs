using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ConteudoAnexoRepository : RespositoryBase<ConteudoAnexoEntity>, IConteudoAnexoRepository
    {
        public ConteudoAnexoRepository(EfContext dbContext)
            : base(dbContext)
        { }

        public List<ConteudoAnexoEntity> Listar(int id)
        {
            return DbContext.ConteudoAnexo.Where(c => c.IdConteudo == id).ToList();
        }
    }
}
