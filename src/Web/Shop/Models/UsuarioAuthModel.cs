using ApplicationCore.Entities;
using Framework.UI.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Shop.Models
{

    [Serializable]
    public sealed class UsuarioAuthModel : IAuthUsuarioIdentificacao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }

        public UsuarioAuthModel() { }

        public UsuarioAuthModel(ClienteEntity model)
        {
            Id = model.Id;
            Nome = model.Nome;
            Email = model.Contato.EmailPessoal;
            Documento = model.Documento;
        }

        public void FillClaims(List<Claim> claim)
        {
            claim.Add(new Claim(nameof(Id), Id.ToString()));
            claim.Add(new Claim(nameof(Nome), Nome));
            claim.Add(new Claim(ClaimTypes.Name, Nome));
            claim.Add(new Claim(nameof(Email), Email));
            claim.Add(new Claim(nameof(Documento), Documento));
        }

        public void Bind(IEnumerable<Claim> claim)
        {
            int.TryParse(claim.FirstOrDefault(c => c.Type == nameof(Id))?.Value ?? string.Empty, out var id);
            Id = id;
            Nome = claim.FirstOrDefault(c => c.Type == nameof(Nome)).Value ?? string.Empty;
            Email = claim.FirstOrDefault(c => c.Type == nameof(Email)).Value ?? string.Empty;
            Documento = claim.FirstOrDefault(c => c.Type == nameof(Documento)).Value ?? string.Empty;
        }
    }
}
