using System.ComponentModel;

namespace Framework.UI.MVC.TagHelpers
{
    public enum EMascara
    {
        [Description("telefone")]
        Telefone,

        [Description("ddd-telefone")]
        DDDTelefone,

        [Description("data")]
        Data,

        [Description("cnpj")]
        Cnpj,

        [Description("cpf")]
        Cpf,

        [Description("cpf-cnpj")]
        CpfCnpj,

        [Description("cep")]
        Cep,

        [Description("numerico")]
        Numerico,

        [Description("numerico-negativo")]
        NumericoNegativo,

        [Description("moeda")]
        Moeda,

        [Description("porcentagem")]
        Porcentagem,

        [Description("hora")]
        Hora,

        [Description("cartao-credito")]
        CartaoCredito,

        [Description("cartao-credito-cco")]
        CodigoCartaoCredito,

        [Description("validade")]
        Validade
    }
}
