using ApplicationCore.Entities;
using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Resources;
using ApplicationCore.Respositories;
using Framework.Extenders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProdutoService : ServiceBase
    {
        private ProdutoResource ProdutoResource => GetService<ProdutoResource>();
        private IProdutoRepository ProdutoRepository => GetService<IProdutoRepository>();
        private IProdutoNpsRepository ProdutoNpsRepository => GetService<IProdutoNpsRepository>();
        private ICloudStorage CloudStorageService => GetService<ICloudStorage>();
        private ICloudQueueService CloudQueueService => GetService<ICloudQueueService>();
        private IEmailClient EmailClient => GetService<IEmailClient>();

        public ProdutoService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }


        public void Excluir(int id) => ProdutoRepository.Excluir(id);

        public void Salvar(ProdutoEntity model) => ProdutoRepository.Salvar(model, model.Id == 0);

        public async Task SalvarImagemAsync(Stream stream, string extensao)
        {
            await CloudStorageService.UploadImagemAsync(extensao, stream);
        }

        public async Task EnviarNps(int idProduto, string email, DateTime? dataLimite)
        {
            var produto = ProdutoRepository.Recuperar(idProduto);

            ProdutoNpsRepository.Salvar(new ProdutoNpsEntity
            {
                IdProduto = idProduto,
                Email = email,
                DataLimite = dataLimite
            });

            var mensagem = produto.Conteudo.Descricao.FormatWith(new
            {
            });

            var assunto = produto.Conteudo.Descricao.FormatWith(new
            {
            });

            await EmailClient.EnviarAsync(new DadosEnvioEmail(email, assunto, mensagem));
        }
    }
}
