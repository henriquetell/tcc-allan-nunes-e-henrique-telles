using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Resources;
using ApplicationCore.Respositories;
using ApplicationCore.Respositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProdutoService : ServiceBase
    {
        private ProdutoResource ProdutoResource => GetService<ProdutoResource>();
        private IProdutoRepository ProdutoRepository => GetService<IProdutoRepository>();
        private IProdutoImagemRepository ProdutoImagemRepository => GetService<IProdutoImagemRepository>();
        private IProdutoSkuRepository ProdutoSkuRepository => GetService<IProdutoSkuRepository>();
        private ICloudStorage CloudStorageService => GetService<ICloudStorage>();
        private ICloudQueueService CloudQueueService => GetService<ICloudQueueService>();
        private IProdutoImagemReadOnlyRepository ImagemReadOnlyRepository =>
            GetService<IProdutoImagemReadOnlyRepository>();


        public ProdutoService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public List<ProdutoDataValue> ListarDestaque() => ProdutoRepository.ListarDestaque();

        public void Excluir(int id) => ProdutoRepository.Excluir(id);

        public void Salvar(ProdutoEntity model) => ProdutoRepository.Salvar(model, model.Id == 0);

        public async Task SalvarImagemAsync(int idProduto, Stream stream, string extensao)
        {
            var ordem = ImagemReadOnlyRepository.RecuperarOrdem(idProduto);

            var model = new ProdutoImagemEntity
            {
                IdProduto = idProduto,
                Ordem = (byte)++ordem,
                Original = $"{Guid.NewGuid().ToString()}{extensao}"
            };

            ProdutoImagemRepository.Salvar(model);

            await CloudStorageService.UploadImagemAsync(model.Original, stream);
        }

        public async Task<int> ExcluirImagemAsync(int id)
        {
            var model = ImagemReadOnlyRepository.Recuperar(id);

            ProdutoImagemRepository.Excluir(id);

            var blobs = new List<string>
            {
                model.Original
            };
            if (!string.IsNullOrWhiteSpace(model.Grande))
                blobs.Add(model.Grande);
            if (!string.IsNullOrWhiteSpace(model.Media))
                blobs.Add(model.Media);
            if (!string.IsNullOrWhiteSpace(model.Pequena))
                blobs.Add(model.Pequena);

            await CloudStorageService.ExcluirImagemAsync(blobs);

            return await Task.FromResult(model.IdProduto);
        }

        public int OrdernarImagens(List<int> imagem)
        {
            var imagens = ImagemReadOnlyRepository.Listar(imagem);
            foreach (var item in imagens)
            {
                var ordem = imagem.IndexOf(item.Id);
                item.Ordem = (byte)++ordem;
                ProdutoImagemRepository.Salvar(item, false);
            }

            return imagens.First().IdProduto;
        }

        public void Salvar(ProdutoSkuEntity model) => ProdutoSkuRepository.Salvar(model, model.Id == 0);

        public List<ProdutoDataValue> Listar() => ProdutoRepository.ListarParaCatalago();
    }
}
