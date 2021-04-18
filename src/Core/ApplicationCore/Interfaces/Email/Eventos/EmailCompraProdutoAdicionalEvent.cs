using ApplicationCore.DataValue.Conteudo;
using ApplicationCore.Respositories;

namespace ApplicationCore.Interfaces.Email.Eventos
{
    internal class EmailCompraProdutoAdicionalEvent : EmailEventBase
    {
       // private IPedidoProdutoAdicionalRepository PedidoProdutoAdicionalRepository => GetService<IPedidoProdutoAdicionalRepository>();

        public override void OnSucesso()
        {
            //var dataSource = ConteudoDataSource<EmailCompraProdutoAdicionalDataValue>();
            //PedidoProdutoAdicionalRepository.SalvarEmailCompraSucesso(dataSource.IdPedidoProdutoAdicional);
        }
    }
}
