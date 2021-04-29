using ApplicationCore.Configurations;
using ApplicationCore.DataValue;
using ApplicationCore.DataValue.Conteudo;
using ApplicationCore.Entities;
using ApplicationCore.Enuns;
using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Respositories;
using Framework.Extenders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProdutoService : ServiceBase
    {
        private IProdutoRepository ProdutoRepository => GetService<IProdutoRepository>();
        private IConteudoRepository ConteudoRepository => GetService<IConteudoRepository>();
        private IProdutoNpsRepository ProdutoNpsRepository => GetService<IProdutoNpsRepository>();
        private ICloudStorage CloudStorageService => GetService<ICloudStorage>();
        private IEmailClient EmailClient => GetService<IEmailClient>();
        private ICloudQueueService CloudQueueService => GetService<ICloudQueueService>();

        public void ProcessarProdutoAvaliacaoNps(int idProduto)
        {
            var produto = ProdutoRepository.Recuperar(idProduto);
            var promotores = new ENotaNps[]
            {
                ENotaNps.PromotorDez,
                ENotaNps.PromotorNove
            };
            var detratores = new ENotaNps[]
            {
                ENotaNps.DetratorUm,
                ENotaNps.DetratorDois,
                ENotaNps.DetratorTres,
                ENotaNps.DetratorQuatro,
                ENotaNps.DetratorCinco,
                ENotaNps.DetratorSeis
            };
            produto.TotalPromotores = ProdutoNpsRepository.TotalPorTipo(idProduto, promotores);
            produto.TotalDetratores = ProdutoNpsRepository.TotalPorTipo(idProduto, detratores);
            ProdutoRepository.Salvar(produto);
        }

        public NpsDataValue RecuperarPorProdutoNps(Guid? id, int? idProduto)
        {
            return ProdutoNpsRepository.RecuperarPorProdutoNps(id, idProduto);
        }

        private ApplicationCoreConfig ApplicationCoreConfig => GetService<ApplicationCoreConfig>();
        public ProdutoService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public async Task SalvarNps(Guid id, ENotaNps nota, string comentario)
        {
            var nps = ProdutoNpsRepository.Recuperar(id);
            nps.Nota = nota;
            nps.Comentario = comentario;
            nps.DataResposta = DateTime.Now;
            ProdutoNpsRepository.Salvar(nps);

            await CloudQueueService.SendAsync(nps.IdProduto, CloudQueueNames.ProcessarNpsQueue);
        }

        public void Excluir(int id) => ProdutoRepository.Excluir(id);

        public void Salvar(ProdutoEntity model) => ProdutoRepository.Salvar(model, model.Id == 0);

        public async Task SalvarImagemAsync(Stream stream, string extensao)
        {
            await CloudStorageService.UploadImagemAsync(extensao, stream);
        }

        public async Task EnviarNps(int idProduto, string email, DateTime? dataLimite)
        {
            var produto = ProdutoRepository.Recuperar(idProduto);
            var conteudo = ConteudoRepository.Recuperar(produto.IdConteudo);
            var nps = new ProdutoNpsEntity
            {
                IdProduto = idProduto,
                Email = email,
                DataLimite = dataLimite,
                DataEnvio = DateTime.Now
            };

            ProdutoNpsRepository.Salvar(nps);

            var mensagem = conteudo.Descricao.FormatWith(new EmailNpsDataValue
            {
                Produto = produto.Titulo,
                Descricao = produto.DescricaoLonga,
                Url = new Uri(ApplicationCoreConfig.UrlPesquisaNps, $"pesquisa/{produto.Id}/{nps.Id}").AbsoluteUri
            });

            var assunto = conteudo.Assunto.FormatWith(new EmailNpsDataValue
            {
                Produto = produto.Titulo
            });

            await EmailClient.EnviarAsync(new DadosEnvioEmail(email, assunto, mensagem));
        }
    }
}
