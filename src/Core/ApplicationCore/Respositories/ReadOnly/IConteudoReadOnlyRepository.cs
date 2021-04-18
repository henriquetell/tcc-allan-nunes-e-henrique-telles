using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using Framework.Data;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IConteudoReadOnlyRepository : IReadOnlyRepository<ConteudoEntity>
    {
        List<ConteudoEntity> Listar(EConteudoChave conteudoChave, string descricao, PaginadorInfo paginador);
        ConteudoEntity Recuperar(EConteudoChave conteudoChave);
    }
}
