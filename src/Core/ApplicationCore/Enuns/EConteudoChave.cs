using ApplicationCore.DataValue.Conteudo;
using Framework.Data;
using System;
using System.Linq;

namespace ApplicationCore.Enuns
{
    partial class EConteudoChave
    {
        public static readonly EConteudoChave EmailPedidoConfirmacao = new EConteudoChave(1, "E-mail de confirmação do pedido", GetDescription(typeof(EmailPedidoDataValue)));
        public static readonly EConteudoChave EmailPedidoPagamentoSucesso = new EConteudoChave(2, "E-mail de pagamento com sucesso", GetDescription(typeof(EmailPedidoDataValue)));
        public static readonly EConteudoChave EmailPedidoPagamentoErro = new EConteudoChave(3, "E-mail de pagamento com erro", GetDescription(typeof(EmailPedidoDataValue)));
        public static readonly EConteudoChave EmailPedidoCancelamento = new EConteudoChave(4, "E-mail de cancelamento", GetDescription(typeof(EmailPedidoDataValue)));
        public static readonly EConteudoChave EmailPedidoEstorno = new EConteudoChave(5, "E-mail de pedido estornado", GetDescription(typeof(EmailPedidoDataValue)));
        public static readonly EConteudoChave EmailCadastroRealizado = new EConteudoChave(6, "E-mail de cadastro realizado", GetDescription(typeof(EmailCadastroDataValue)));
        public static readonly EConteudoChave EmailAlteracaoCadastro = new EConteudoChave(6, "E-mail de alteração de senha", GetDescription(typeof(EmailAlteracaoSenhaDataValue)));
        private static string GetDescription(Type type, string prefix = "\t")
        {
            var props = type.GetProperties()
                .OrderBy(c => c.Name)
                .Select(c =>
                {
                    if (c.PropertyType.IsPrimitive || c.PropertyType.IsValueType || c.PropertyType == typeof(string))
                        return $"<b>{c.Name}</b>: {c.PropertyType};";

                    return $"<b>{c.Name}</b>: {GetDescription(c.PropertyType, prefix + '\t')}";
                }).ToList();

            return $@"{type.Name} {{{Environment.NewLine + prefix}{string.Join(Environment.NewLine + prefix, props)}{Environment.NewLine + prefix}}}";
        }
    }

    [Serializable]
    public partial class EConteudoChave : CustomEnumeration<EConteudoChave, int>
    {
        public readonly int Id;
        public readonly string Titulo;
        public readonly string Descricao;
        public readonly bool Email;
        public readonly string UrlFriendly;

        private EConteudoChave(int id, string titulo, string descricao = null,
            string urlFriendly = null,
            bool email = true)
            : base(id, titulo)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao ?? titulo;
            UrlFriendly = urlFriendly;
            Email = email;
        }

        public static explicit operator EConteudoChave(int id) => GetAll().FirstOrDefault(c => c.Id == id);

        public static EConteudoChave TryParseByUrlFriendly(string urlFriendly) => GetAll().FirstOrDefault(c => c.UrlFriendly == urlFriendly);
    }
}
