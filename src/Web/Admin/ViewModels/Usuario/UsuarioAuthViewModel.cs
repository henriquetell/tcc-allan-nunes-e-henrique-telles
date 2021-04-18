using ApplicationCore.Entities;
using Framework.UI.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Admin.ViewModels.Usuario
{
    [Serializable]
    public class UsuarioAuthViewModel : IAuthUsuarioIdentificacao
    {
        public int Id { get; set; }
        public int IdGrupoUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Imagem { get; set; }

        public string GrupoAcesso { get; set; }

        public UsuarioAuthViewModel() { }

        public UsuarioAuthViewModel(UsuarioEntity model)
        {
            Id = model.Id;
            Nome = model.Nome;
            Email = model.Email;
            Imagem = model.Imagem;
            GrupoAcesso = model.GrupoUsuario?.Nome;
            IdGrupoUsuario = model.IdGrupoUsuario;
        }

        public void FillClaims(List<Claim> claim)
        {
            claim.Add(new Claim(nameof(Id), Id.ToString()));
            claim.Add(new Claim(nameof(Nome), Nome));
            claim.Add(new Claim(ClaimTypes.Name, Nome));
            claim.Add(new Claim(nameof(Email), Email));
            claim.Add(new Claim(nameof(Cpf), Cpf ?? string.Empty));
            claim.Add(new Claim(nameof(GrupoAcesso), GrupoAcesso ?? string.Empty));
            claim.Add(new Claim(nameof(IdGrupoUsuario), IdGrupoUsuario.ToString()));
        }

        public void Bind(IEnumerable<Claim> claim)
        {
            if (int.TryParse(claim.FirstOrDefault(c => c.Type == nameof(Id))?.Value ?? string.Empty, out var id))
                Id = id;

            if (int.TryParse(claim.FirstOrDefault(c => c.Type == nameof(IdGrupoUsuario))?.Value ?? string.Empty, out var idGrupoUsuario))
                IdGrupoUsuario = idGrupoUsuario;

            Nome = claim.FirstOrDefault(c => c.Type == nameof(Nome)).Value ?? string.Empty;
            Email = claim.FirstOrDefault(c => c.Type == nameof(Email)).Value ?? string.Empty;
            Cpf = claim.FirstOrDefault(c => c.Type == nameof(Cpf)).Value ?? string.Empty;
            GrupoAcesso = claim.FirstOrDefault(c => c.Type == nameof(GrupoAcesso)).Value ?? string.Empty;
        }
    }
}
