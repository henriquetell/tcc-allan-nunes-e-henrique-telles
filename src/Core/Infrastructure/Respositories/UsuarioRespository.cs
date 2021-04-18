using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using Framework.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class UsuarioRespository : RespositoryBase<UsuarioEntity>, IUsuarioRepository
    {
        public UsuarioRespository(EfContext dbContext)
            : base(dbContext)
        { }

        public UsuarioEntity RecuperarPorEmail(string email) => DbContext.Usuario.Include(c => c.GrupoUsuario).FirstOrDefault(u => u.Email == email);

        public List<UsuarioEntity> Listar(string consulta, PaginadorInfo paginador)
        {
            var query = DbContext.Usuario
                .OrderBy(c => c.Status)
                .ThenBy(c => c.Nome)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(consulta))
                query = query.Where(c => c.Email.Contains(consulta) || c.Nome.Contains(consulta));

            return Paginar(query, paginador);
        }
    }
}
