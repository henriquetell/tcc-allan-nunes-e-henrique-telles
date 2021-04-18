using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Interfaces.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.CloudServices
{
    public class AzureStorage : CloudServiceBase, ICloudStorage
    {
        private readonly IAppLogger _appLogger;

        public AzureStorage(IServiceProvider serviceProvider)
               : base(serviceProvider)
        {
            _appLogger = serviceProvider.GetService<IAppLogger<AzureStorage>>();
        }

        private StorageConfig StorageConfig => GetService<InfrastructureConfig>().Storage;

        public Uri RecuperarImagemUrl(string nomeBlob)
           => new Uri(new Uri(StorageConfig.Url), Path.Combine(StorageConfig.DiretorioImagensProduto, nomeBlob ?? string.Empty));

        public Task RecuperarImagemAsync(string nomeBlob, out string path)
           => DownloadAsync(nomeBlob, StorageConfig.DiretorioImagensProduto, out path);

        public Task UploadAnexoAsync(string nomeBlob, Stream arquivo) =>
            UploadAsync(nomeBlob, arquivo, StorageConfig.DiretorioConteudoAnexo);

        public Task ExcluirAnexoAsync(string nomeBlob)
            => ExcluirAsync(nomeBlob, StorageConfig.DiretorioConteudoAnexo);

        public Uri RecuperarAnexoUrl(string nomeBlob) => new Uri(new Uri(StorageConfig.Url), Path.Combine(StorageConfig.DiretorioConteudoAnexo, nomeBlob));

        #region Métodos - Base

        public async Task UploadAsync(string nomeBlob, Stream arquivo, string diretorio, bool? sobrescrever = null)
        {
            var blobContainer = GetBlobContainer(diretorio);

            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var destBlob = blobContainer.GetBlobClient(nomeBlob);
            if (sobrescrever ?? false)
            {
                var deleted = await destBlob.DeleteIfExistsAsync();
                _appLogger.InfoIf(deleted, $"O blob {nomeBlob} existia e foi excluído");
            }

            var timer = _appLogger.Warning($"Enviando {nomeBlob} para {diretorio}").StartTimer();
            await destBlob.UploadAsync(arquivo);
            timer.Stop((t, l) => l.Warning($"Tempo para envio do arquivo: {TimeSpan.FromMilliseconds(t)}"));
        }

        public async Task<string> UploadAsync(string nomeBlob, string body, string diretorio, bool? sobrescrever = null)
        {
            var blobContainer = GetBlobContainer(diretorio);

            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var destBlob = blobContainer.GetBlobClient(nomeBlob);
            if (sobrescrever ?? false)
            {
                var deleted = await destBlob.DeleteIfExistsAsync();
                _appLogger.InfoIf(deleted, $"O blob {nomeBlob} existia e foi excluído");
            }

            var timer = _appLogger.Warning($"Enviando {nomeBlob} para {diretorio}").StartTimer();
            using var stream = new MemoryStream(Encoding.ASCII.GetBytes(body));
            var info = await destBlob.UploadAsync(stream);
            timer.Stop((t, l) => l.Warning($"Tempo para envio do arquivo: {TimeSpan.FromMilliseconds(t)}"));
            return $"{StorageConfig.Url}{diretorio}/{nomeBlob}";
        }

        public Task ExcluirAsync(string nomeBlob, string diretorio) =>
            GetCloudBlockBlob(nomeBlob, diretorio).DeleteIfExistsAsync();

        public async Task ExcluirAsync(List<string> nomeBlob, string diretorio)
        {
            var blobContainer = GetBlobContainer(diretorio);
            foreach (var item in nomeBlob)
            {
                await blobContainer.DeleteBlobIfExistsAsync(item);
            }
        }

        public Task DownloadAsync(string nomeBlob, string diretorio, Stream stream)
        {
            var destBlob = GetCloudBlockBlob(nomeBlob, diretorio);
            return destBlob.DownloadToAsync(stream);
        }

        public Task DownloadAsync(string nomeBlob, string diretorio, out string pathFile)
        {
            var destBlob = GetCloudBlockBlob(nomeBlob, diretorio);

            pathFile = Path.GetTempFileName() + Path.GetExtension(nomeBlob);

            return destBlob.DownloadToAsync(pathFile);
        }

        private BlobContainerClient GetBlobContainer(string diretorio)
        {
            diretorio = SanitizarNomeDiretorio(diretorio);

            var blobClient = new BlobServiceClient(StorageConfig.ConnectionString);
            var blobContainer = blobClient.GetBlobContainerClient(diretorio);
            return blobContainer;
        }

        private BlobClient GetCloudBlockBlob(string nomeBlob, string diretorio) =>
            GetBlobContainer(diretorio).GetBlobClient(nomeBlob);


        private static string SanitizarNomeDiretorio(string nomeDiretorio)
        {
            nomeDiretorio = (nomeDiretorio ?? "").ToLower();
            nomeDiretorio = Regex.Replace(nomeDiretorio, "[^a-z0-9\\-]", "").Trim('-');
            nomeDiretorio = Regex.Replace(nomeDiretorio, "\\-{1,999}", "-");
            return nomeDiretorio;
        }

        #endregion

        #region Imagem

        public Task UploadImagemAsync(string nomeBlob, Stream arquivo) =>
            UploadAsync(nomeBlob, arquivo, StorageConfig.DiretorioImagensProduto);

        public Task ExcluirImagemAsync(List<string> nomeBlob)
        => ExcluirAsync(nomeBlob, StorageConfig.DiretorioImagensProduto);

        #endregion

    }
}
