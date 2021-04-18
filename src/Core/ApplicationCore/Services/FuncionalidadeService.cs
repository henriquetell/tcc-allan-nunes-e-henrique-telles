using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Security.Authorization;
using ApplicationCore.Respositories;

namespace ApplicationCore.Services
{
    public class FuncionalidadeService : ServiceBase
    {
        private IPermissaoRepository PermissaoRepository => GetService<IPermissaoRepository>();
        private IPermissaoAcaoRepository PermissaoAcaoRepository => GetService<IPermissaoAcaoRepository>();

        public FuncionalidadeService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public void Sincronizar(List<AuthPermissao> funcionalidades)
        {
            var permissoes = funcionalidades.Select(f => new PermissaoEntity
            {
                Id = f.Id,
                Descricao = f.Descricao,
                NomeGrupo = f.Grupo
            });
            var acoes = funcionalidades.SelectMany(f => f.Acoes).Select(c => new PermissaoAcaoEntity
            {
                Id = c.Id,
                IdPermissao = c.Funcionalidade.Id,
                TipoAcao = (int)c.Acao
            });

            PermissaoRepository.Sincronizar(permissoes);
            PermissaoAcaoRepository.Sincronizar(acoes);
        }
    }
}
