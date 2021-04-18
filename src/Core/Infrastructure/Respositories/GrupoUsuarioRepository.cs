using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using Framework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class GrupoUsuarioRepository : RespositoryBase<GrupoUsuarioEntity>, IGrupoUsuarioRepository
    {
        public GrupoUsuarioRepository(EfContext dbContext)
            : base(dbContext)
        { }

        public override GrupoUsuarioEntity Recuperar(int id) =>
            DbContext.GrupoUsuario
                .Include(c => c.GrupoUsuarioPermisaoAcao)
                .Include(c => c.Usuario)
                .FirstOrDefault(c => c.Id == id);

        public GrupoUsuarioEntity RecuperarPorNome(string nomeGrupo) => DbContext.GrupoUsuario.FirstOrDefault(g => g.Nome == nomeGrupo);

        public List<GrupoUsuarioEntity> Listar(string consulta, PaginadorInfo paginador)
        {
            var query = DbContext.GrupoUsuario.AsQueryable();
            if (!string.IsNullOrEmpty(consulta))
                query = query.Where(c => c.Nome.Contains(consulta));
            return Paginar(query, paginador);
        }

        public List<Guid> ListarPermissaoAcaoPorUsuario(int idUsuario)
        {
            return DbContext.GrupoUsuario.Where(c => c.Usuario.Any(y => y.Id == idUsuario))
                .SelectMany(c => c.GrupoUsuarioPermisaoAcao.Select(y => y.IdPermissaoAcao)).ToList();
        }
    }
}
