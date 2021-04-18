using ApplicationCore.Entities;
using ApplicationCore.Enuns;

namespace Shop.ViewModels.Common
{
    public class EnderecoViewModel
    {
        public EnderecoViewModel()
        { }

        public EnderecoViewModel(EnderecoCliente endereco)
        {
            Logradouro = endereco.Logradouro;
            Complemento = endereco.Complemento;
            Numero = endereco.Numero;
            Cep = endereco.Cep;
            Bairro = endereco.Bairro;
            Cidade = endereco.Cidade;
            Uf = endereco.Uf;
        }

        public void Fill(EnderecoCliente endereco)
        {
            Logradouro = endereco.Logradouro;
            Complemento = endereco.Complemento;
            Numero = endereco.Numero;
            Cep = endereco.Cep;
            Bairro = endereco.Bairro;
            Cidade = endereco.Cidade;
            Uf = endereco.Uf;
        }

        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public EUf? Uf { get; set; }
    }
}
