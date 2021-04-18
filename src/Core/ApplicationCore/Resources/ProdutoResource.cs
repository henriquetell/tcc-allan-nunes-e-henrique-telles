using Framework.Resources;
using Microsoft.Extensions.Localization;

namespace ApplicationCore.Resources
{
    public class ProdutoResource : ResourceBase
    {
        public virtual LocalizedString SemEstoque => Resource("Erro001", "Não existe estoque disponível para a opção: \"{0}\"");
        public virtual LocalizedString PagamentoErroOperadora => Resource("Erro002", "Não foi possível efetuar o pagamento da cobrança, verifique se os dados do cartão de credito estão corretos, ou entre em contato com a operadora.");
        public virtual LocalizedString PagamentoErroInterno => Resource("Erro003", "Não foi possível efetuar o pagamento da cobrança, tente novamente mais tarde.");
        public virtual LocalizedString EstoqueErroMovimentacao => Resource("Erro004", "Não foi possível efetuar a movimentação do estoque.");
    }
}
