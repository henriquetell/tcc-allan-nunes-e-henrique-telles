using Admin.ViewModels.Conteudo;
using ApplicationCore.Respositories.ReadOnly;
using System;
using System.Linq;

namespace Admin.Services
{
    public class ConteudoServiceWeb : BaseServiceWeb
    {
        private IConteudoReadOnlyRepository ConteudoReadOnlyRepository => GetService<IConteudoReadOnlyRepository>();
        private IConteudoAnexoReadOnlyRepository ConteudoAnexoReadOnlyRepository => GetService<IConteudoAnexoReadOnlyRepository>();

        public ConteudoServiceWeb(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public ConteudoFiltroViewModel Listar(ConteudoFiltroViewModel filtro)
        {
            var itens = ConteudoReadOnlyRepository.Listar(filtro.ConteudoChave, filtro.Descricao, filtro.Paginador);

            filtro.Itens = itens.Select(c => new ConteudoViewModel
            {
                Id = c.Id,
                Descricao = c.Descricao,
                ConteudoChave = c.ConteudoChave,
                Status = c.Status,
                IdConteudoChave = c.IdConteudo,
                Titulo = c.Titulo,
                Assunto = c.Assunto
            })
                .OrderBy(c => c.Descricao)
                .ToList();

            return filtro;
        }

        public ConteudoViewModel Recuperar(int id)
        {
            var model = ConteudoReadOnlyRepository.Recuperar(id);
            if (model == null)
                return new ConteudoViewModel();

            var anexos = ConteudoAnexoReadOnlyRepository.Listar(id);

            return new ConteudoViewModel
            {
                Id = model.Id,
                ConteudoChave = model.ConteudoChave,
                IdConteudoChave = model.IdConteudo,
                Status = model.Status,
                Descricao = model.Descricao,
                Titulo = model.Titulo,
                Assunto = model.Assunto,
                Anexos = anexos.Select(c => new ConteudoAnexoViewModel
                {
                    Id = c.Id,
                    IdConteudo = c.IdConteudo,
                    NomeOriginal = c.NomeOriginal,
                    Anexo = c.Anexo
                }).ToList()
            };
        }
    }
}
