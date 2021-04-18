using Admin.Resources;
using ApplicationCore.Enuns;
using Framework.Security.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.GrupoUsuario
{
    public class GrupoUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Nome { get; set; }

        public List<AuthPermissao> Permissoes => AuthPermissao.Listar();

        public Guid[] PermissoesDoGrupo { get; set; } = { };
    }
}
