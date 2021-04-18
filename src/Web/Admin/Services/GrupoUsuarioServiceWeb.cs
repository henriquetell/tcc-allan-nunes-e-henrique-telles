using Admin.ViewModels.GrupoUsuario;
using ApplicationCore.Respositories.ReadOnly;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Services
{
    public class GrupoUsuarioServiceWeb : BaseServiceWeb
    {
        private IGrupoUsuarioReadOnlyRepository GrupoUsuarioReadOnlyRepository =>
            GetService<IGrupoUsuarioReadOnlyRepository>();

        public GrupoUsuarioServiceWeb(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public GrupoUsuarioViewModel Recuperar(int id)
        {
            var model = GrupoUsuarioReadOnlyRepository.Recuperar(id);
            if (model == null)
                return new GrupoUsuarioViewModel();

            return new GrupoUsuarioViewModel
            {
                Id = model.Id,
                Nome = model.Nome,
                Status = model.Status,
                PermissoesDoGrupo = model.GrupoUsuarioPermisaoAcao.Select(c => c.IdPermissaoAcao).ToArray()
            };
        }

        public GrupoUsuarioFiltroViewModel Listar(GrupoUsuarioFiltroViewModel filtro)
        {
            var itens = GrupoUsuarioReadOnlyRepository.Listar(filtro.Consulta, filtro.Paginador);

            filtro.Itens = itens.Select(c => new GrupoUsuarioViewModel
            {
                Nome = c.Nome,
                Status = c.Status,
                Id = c.Id
            }).ToList();

            return filtro;
        }

        public IEnumerable<SelectListItem> Listar()
        {
            var itens = GrupoUsuarioReadOnlyRepository.Listar();

            return itens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome,
                Group = new SelectListGroup { Name = c.Status.GetDescription() }
            });
        }
    }
}
