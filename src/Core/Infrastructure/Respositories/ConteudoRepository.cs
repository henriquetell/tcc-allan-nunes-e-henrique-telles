using ApplicationCore.DataValue.Common;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories;
using Framework.Data;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class ConteudoRepository : RespositoryBase<ConteudoEntity>, IConteudoRepository
    {
        public ConteudoRepository(EfContext dbContext)
            : base(dbContext)
        { }

        public List<ConteudoEntity> Listar(EConteudoChave conteudoChave, string filtro, PaginadorInfo paginador)
        {
            var consulta = DbContext.Conteudo
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
                consulta = consulta.Where(c => c.Assunto.Contains(filtro) ||
               c.Descricao.Contains(filtro) ||
               c.Titulo.Contains(filtro));

            if (conteudoChave != null)
                consulta = consulta.Where(c => c.IdConteudo == conteudoChave.Id);

            return Paginar(consulta, paginador);
        }

        public List<SelectListItemDataValue<EStatus>> ListarParaSelect()
        {
            return DbContext.Conteudo.Select(c => new SelectListItemDataValue<EStatus>
            {
                Value = c.Id.ToString(),
                Text = c.Titulo,
                Enum = c.Status
            })
                .ToList();
        }

        public ConteudoEntity Recuperar(EConteudoChave conteudoChave)
        {
            return DbContext.Conteudo
                .Where(c =>
                            c.IdConteudo == conteudoChave.Id &&
                            c.Status == EStatus.Ativo)
                .OrderByDescending(c => c.Id)
                .FirstOrDefault();
        }
    }
}
