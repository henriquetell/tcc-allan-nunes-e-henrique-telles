using Admin.ViewModels.Produto;
using ApplicationCore.Respositories.ReadOnly;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Services
{
    public class ProdutoServiceWeb : BaseServiceWeb
    {
        private IProdutoReadOnlyRepository ProdutoReadOnlyRepository =>
            GetService<IProdutoReadOnlyRepository>();

        private IProdutoNpsReadOnlyRepository ImagemReadOnlyRepository =>
            GetService<IProdutoNpsReadOnlyRepository>();

        public ProdutoServiceWeb(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public ProdutoViewModel Recuperar(int idProduto)
        {
            var model = ProdutoReadOnlyRepository.Recuperar(idProduto, true);
            var vm = new ProdutoViewModel();
            vm.Fill(model);
            return vm;
        }

        public ProdutoFiltroViewModel Listar(ProdutoFiltroViewModel filtro)
        {
            var itens = ProdutoReadOnlyRepository.Listar(filtro.Consulta, filtro.Paginador);

            filtro.Itens = itens.Select(model => new ProdutoViewModel
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Status = model.Status,
                ImagemUrl = model.Imagem,
                TotalDetratores = model.TotalDetratores,
                TotalPromotores = model.TotalPromotores
            }).ToList();

            return filtro;
        }

        public IEnumerable<SelectListItem> Listar()
        {
            var itens = ProdutoReadOnlyRepository.Listar();

            return itens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Titulo,
                Group = new SelectListGroup { Name = c.Status.GetDescription() }
            });
        }
    }
}
