using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Resources;
using ApplicationCore.Respositories;
using Framework.Configurations;
using Framework.Exceptions;
using Framework.Extenders;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class UsuarioService : ServiceBase
    {
        private IUsuarioRepository UsuarioRepositorio => GetService<IUsuarioRepository>();
        private UsuarioResource UsuarioResource => GetService<UsuarioResource>();
        private IEmailClient EmailClient => GetService<IEmailClient>();
        private IMemoryCache MemoryCache => GetService<IMemoryCache>();

        public UsuarioService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public UsuarioEntity Autenticar(string email, string senha)
        {
            var usuario = UsuarioRepositorio.RecuperarPorEmail(email);

            if (usuario == null)
                throw new MensagemException(UsuarioResource.NaoLocalizado);

            if (usuario.Status == EStatus.Inativo)
                throw new MensagemException(UsuarioResource.Inativo);

            if (usuario.Senha != CriptografarSenha(senha, usuario.Salt))
                throw new MensagemException(UsuarioResource.SenhaErrada);

            return usuario;
        }

        public void Excluir(int id) => UsuarioRepositorio.Excluir(id);

        public async Task Salvar(UsuarioEntity model, string conteudoEmail)
        {
            string senha = null;
            var usuario = UsuarioRepositorio.Recuperar(model.Id) ?? model;

            if (usuario.Id == 0)
            {
                if (UsuarioRepositorio.RecuperarPorEmail(model.Email) != null)
                    throw new MensagemException(UsuarioResource.UsuarioJaPossuiCadastro);

                senha = GerarSenha();
                usuario.Salt = GerarSalt();
                usuario.Senha = CriptografarSenha(senha, usuario.Salt);
            }
            else
            {
                usuario.IdGrupoUsuario = model.IdGrupoUsuario;
                usuario.Nome = model.Nome;
                usuario.Email = model.Email;
                usuario.Status = model.Status;
            }

            UsuarioRepositorio.Salvar(usuario, usuario.Id == 0);

            if (senha == null)
                return;

            var body = conteudoEmail.FormatWith(new
            {
                senha,
                email = model.Email,
                url = AppConfig.Admin.Url
            });

            await EmailClient.EnviarAsync(new DadosEnvioEmail(model.Email, "Bem-vindo! Dados de acesso!", body));
        }

        public void SalvarImagem(int id, string imagem)
        {
            var model = UsuarioRepositorio.Recuperar(id);
            if (model == null)
                throw new MensagemException(UsuarioResource.NaoLocalizado);

            model.Imagem = imagem;

            UsuarioRepositorio.Salvar(model, false);

            MemoryCache.Remove($"ImagemUsuario|{id}");
        }

        public async Task AlterarSenha(int idUsuario, string senha, string novaSenha, string confirmacaoNovaSenha, string conteudoEmail)
        {
            var model = UsuarioRepositorio.Recuperar(idUsuario);
            if (model == null)
                throw new MensagemException(UsuarioResource.NaoLocalizado);

            if (novaSenha != confirmacaoNovaSenha)
                throw new MensagemException(UsuarioResource.SenhaEConfirmacaoNaoConferem);

            if (model.Senha != CriptografarSenha(senha, model.Salt))
                throw new MensagemException(UsuarioResource.SenhaErradaAlteracao);

            model.Salt = GerarSalt();
            model.Senha = CriptografarSenha(novaSenha, model.Salt);

            UsuarioRepositorio.Salvar(model, false);

            var body = conteudoEmail.FormatWith(new
            {
                senha = novaSenha,
                url = AppConfig.Admin.Url
            });

            await EmailClient.EnviarAsync(new DadosEnvioEmail(model.Email, "Nova Senha! Você alterou sua senha!", body));

        }

        internal static string CriptografarSenha(string senha, byte[] salt) => Convert.ToBase64String(KeyDerivation.Pbkdf2(senha, salt, KeyDerivationPrf.HMACSHA1, 10000,
            256 / 8));

        internal static byte[] GerarSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            return salt;
        }

        internal static string GerarSenha() => new Random().Next(100000, 999999).ToString();
    }
}
