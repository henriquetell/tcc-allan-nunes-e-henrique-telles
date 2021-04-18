using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.CloudServices.CloudStorage
{
    public interface ICloudStorage
    {
        Uri RecuperarImagemUrl(string nomeBlob);
        Uri RecuperarAnexoUrl(string nomeBlob);


        Task UploadImagemAsync(string nomeBlob, Stream arquivo);
        Task RecuperarImagemAsync(string nomeBlob, out string path);

        Task ExcluirImagemAsync(List<string> nomeBlob);

        Task UploadAnexoAsync(string nomeBlob, Stream arquivo);

        Task ExcluirAnexoAsync(string nomeBlob);

        Task UploadAsync(string nomeBlob, Stream arquivo, string diretorio, bool? sobrescrever = null);
    }
}
