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

        private IProdutoImagemReadOnlyRepository ImagemReadOnlyRepository =>
            GetService<IProdutoImagemReadOnlyRepository>();

        private IProdutoSkuReadOnlyRepository ProdutoSkuReadOnlyRepository =>
            GetService<IProdutoSkuReadOnlyRepository>();

        public ProdutoServiceWeb(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public List<SelectListItem> ListarSkus(int idProduto)
        {
            if (idProduto <= 0)
                return new List<SelectListItem>();

            var itens = ProdutoSkuReadOnlyRepository.ListarParaFiltro(idProduto);

            return itens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Titulo} - {c.Descricao}",
                Group = new SelectListGroup
                {
                    Name = c.Status.GetDescription()
                }
            }).ToList();
        }

        public ProdutoViewModel Recuperar(int idProduto)
        {
            var model = ProdutoReadOnlyRepository.Recuperar(idProduto, true);
            var vm = new ProdutoViewModel();
            vm.Fill(model);
            var skus = ProdutoSkuReadOnlyRepository.Listar(idProduto);
            vm.Skus.AddRange(skus.Select(s => new ProdutoSkuViewModel(s)));
            vm.Skus.Add(new ProdutoSkuViewModel
            {
                IdProduto = idProduto
            });

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
                ImagemPrincipal = model.ProdutoImagem.OrderBy(c => c.Ordem).FirstOrDefault()?.Original
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

        public IEnumerable<ProdutoImagemViewModel> Listar(int id)
        {
            return ImagemReadOnlyRepository.ListarPorId(id).Select(c =>
                new ProdutoImagemViewModel
                {
                    Id = c.Id,
                    IdProduto = c.IdProduto,
                    Grande = c.Grande,
                    Media = c.Media,
                    Pequena = c.Pequena,
                    Ordem = c.Ordem,
                    Original = c.Original
                });
        }
    }
}
