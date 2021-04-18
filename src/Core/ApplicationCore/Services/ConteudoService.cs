using ApplicationCore.Entities;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Respositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ConteudoService : ServiceBase
    {
        private IConteudoRepository ConteudoRepository => GetService<IConteudoRepository>();
        private IConteudoAnexoRepository ConteudoAnexoRepository => GetService<IConteudoAnexoRepository>();
        private ICloudStorage CloudStorageService => GetService<ICloudStorage>();
        private IMemoryCache Cache => GetService<IMemoryCache>();

        public ConteudoService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public void Salvar(ConteudoEntity model)
        {
            ConteudoRepository.Salvar(model, model.Id == 0);
            Cache.Remove($"Conteudo|{model.Id}");
        }

        public Task SalvarAnexoAsync(int idConteudo, Stream stream, string nomeOriginal, string extensao)
        {
            var model = new ConteudoAnexoEntity
            {
                IdConteudo = idConteudo,
                NomeOriginal = nomeOriginal,
                Anexo = $"{Guid.NewGuid()}{extensao}"
            };

            ConteudoAnexoRepository.Salvar(model);

            return CloudStorageService.UploadAnexoAsync(model.Anexo, stream);
        }

        public async Task<int> ExcluirAnexoAsync(int id)
        {
            var model = ConteudoAnexoRepository.Recuperar(id);

            ConteudoAnexoRepository.Excluir(id);

            await CloudStorageService.ExcluirAnexoAsync(model.Anexo);

            return await Task.FromResult(model.IdConteudo);
        }
    }
}
