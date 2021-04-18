using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using Framework.Extenders;
using Shop.Resources;
using Shop.ViewModels.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels.MinhaConta
{
    public class MinhaContaViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Documento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [EmailAddress(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.EmailInvalido))]
        public string Email { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.EmailInvalido))]
        public string EmailSecundario { get; set; }

        public bool AlterarSenha { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,12}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.NivelSegurancaSenha))]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,12}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.NivelSegurancaSenha))]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{6,12}$", ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.NivelSegurancaSenha))]
        [DataType(DataType.Password)]
        [Compare(nameof(NovaSenha), ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.ConfimacaoSenha))]
        public string ConfirmarNovaSenha { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Celular { get; set; }

        public string TelefoneResidencial { get; set; }
        public string TelefoneComercial { get; set; }

        public EnderecoViewModel Endereco { get; set; } = new EnderecoViewModel();

        public void Fill(ClienteEntity cliente)
        {
            Nome = cliente.Nome;
            Documento = cliente.Documento;
            DataNascimento = cliente.DataNascimento;
            Email = cliente.Contato.EmailPessoal;
            EmailSecundario = cliente.Contato.EmailSecundario;
            Celular = cliente.Contato.Celular;
            TelefoneComercial = cliente.Contato.TelefoneComercial;
            TelefoneResidencial = cliente.Contato.TelefoneResidencial;
        }

        public ClienteDataValue GetDataValue()
        {
            return new ClienteDataValue
            {
                Nome = Nome,
                Email = Email,
                EmailSecundario = EmailSecundario,
                AlterarSenha = AlterarSenha,
                Senha = Senha,
                NovaSenha = NovaSenha,
                ConfirmarNovaSenha = ConfirmarNovaSenha,
                DataNascimento = DataNascimento,
                Documento = Documento.RemoveNaoNumericos(),
                Celular = Celular.RemoveNaoNumericos(),
                TelefoneResidencial = TelefoneResidencial.RemoveNaoNumericos(),
                TelefoneComercial = TelefoneComercial.RemoveNaoNumericos(),

                Logradouro = Endereco.Logradouro,
                Complemento = Endereco.Complemento,
                Numero = Endereco.Numero,
                Cep = Endereco.Cep.RemoveNaoNumericos(),
                Bairro = Endereco.Bairro,
                Cidade = Endereco.Cidade,
                Uf = Endereco.Uf
            };
        }
    }
}
