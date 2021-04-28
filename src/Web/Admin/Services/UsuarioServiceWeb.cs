using Admin.ViewModels.Usuario;
using ApplicationCore.Respositories.ReadOnly;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Services
{
    public class UsuarioServiceWeb : BaseServiceWeb
    {
        private IUsuarioReadOnlyRepository UsuarioReadOnlyRepository => GetService<IUsuarioReadOnlyRepository>();

        public UsuarioServiceWeb(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public UsuarioViewModel Recuperar(int id)
        {
            var model = UsuarioReadOnlyRepository.Recuperar(id);
            if (model == null)
                return new UsuarioViewModel();

            return new UsuarioViewModel
            {
                Nome = model.Nome,
                Status = model.Status,
                Id = model.Id,
                Email = model.Email,
                IdGrupoUsuario = model.IdGrupoUsuario,
                Imagem = model.Imagem
            };
        }

        public UsuarioFiltroViewModel Listar(UsuarioFiltroViewModel filtro)
        {
            var itens = UsuarioReadOnlyRepository.Listar(filtro.Consulta, filtro.Paginador);

            filtro.Itens = itens.Select(c => new UsuarioViewModel
            {
                Nome = c.Nome,
                Status = c.Status,
                Id = c.Id,
                Email = c.Email,
                IdGrupoUsuario = c.IdGrupoUsuario,
                Imagem = c.Imagem
            }).ToList();

            return filtro;
        }

        public List<SelectListItem> Listar()
        {
            var itens = UsuarioReadOnlyRepository.Listar();

            return itens.Select(c => new SelectListItem
            {
                Text = c.Nome,
                Value = c.Id.ToString(),
                Group = new SelectListGroup
                {
                    Name = c.Status.GetDescription()
                }
            }).ToList();
        }
    }
}
