using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class GrupoUsuarioService : ServiceBase
    {
        private IGrupoUsuarioRepository GrupoUsuarioCudOperation => GetService<IGrupoUsuarioRepository>();
        private IGrupoUsuarioPermisaoAcaoRepository GrupoUsuarioPermisaoAcaoCudOperation => GetService<IGrupoUsuarioPermisaoAcaoRepository>();
        private IMemoryCache Cache => GetService<IMemoryCache>();
        private IGrupoUsuarioRepository GrupoUsuarioRepository => GetService<IGrupoUsuarioRepository>();

        public GrupoUsuarioService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public void Excluir(int id)
        {
            GrupoUsuarioCudOperation.Excluir(id);
            Cache.Remove($"Permissoes|{id}");
        }

        public List<Guid> ListarPermissaoAcaoPorUsuario(int idUsuario, int idGrupoAcesso)
        {
            return Cache.GetOrCreate($"Permissoes|{idGrupoAcesso}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(5);
                return GrupoUsuarioRepository.ListarPermissaoAcaoPorUsuario(idUsuario);
            });
        }
        public void Salvar(GrupoUsuarioEntity model, Guid[] permissoesDoGrupo)
        {
            GrupoUsuarioCudOperation.Salvar(model, model.Id == 0);
            GrupoUsuarioPermisaoAcaoCudOperation.ExcluirTodos(model.Id);
            var permissoes = permissoesDoGrupo.Select(idPermissaoAcao => new GrupoUsuarioPermisaoAcaoEntity
            {
                IdGrupoUsuario = model.Id,
                IdPermissaoAcao = idPermissaoAcao
            });
            GrupoUsuarioPermisaoAcaoCudOperation.SalvarTodos(permissoes);
            Cache.Remove($"Permissoes|{model.Id}");
        }
    }
}
