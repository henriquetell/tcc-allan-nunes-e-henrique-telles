using System;
using Admin.Filters;
using Admin.ViewModels.StatusCode;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class ErroController : BaseController
    {
        [AuthUsuarioFilter(false)]
        public IActionResult Index() => View();

        [AuthUsuarioFilter(false)]
        public new IActionResult StatusCode(int id)
        {
            var viewModel = new StatusCodeViewModel { StatusCode = id };

            switch (id)
            {
                case 401:
                    viewModel.Titulo = "Não autorizado";
                    viewModel.Mensagem = "Você não está autenticado ou autorizado.";
                    break;
                case 404:
                    viewModel.Titulo = "Não encontrado";
                    viewModel.Mensagem = "O registro requisitado não foi encontrado.";
                    break;
                case 501:
                    viewModel.Titulo = "Não implementado";
                    viewModel.Mensagem = "O servidor ainda não suporta a funcionalidade solicitada.";
                    break;
                case 502:
                    viewModel.Titulo = "Bad Gateway";
                    viewModel.Mensagem = "Ocorreu um erro de comunicação com o servidor.";
                    break;
                case 503:
                    viewModel.Titulo = "Serviço indisponível";
                    viewModel.Mensagem = "O servidor está em manutenção.";
                    break;
                case 504:
                    viewModel.Titulo = "Gateway Time-Out";
                    viewModel.Mensagem = "O tempo de conexão expirou.";
                    break;
                default:
                    viewModel.Titulo = "Código não cadastrado";
                    viewModel.Mensagem = "O código do erro HTTP não está cadastrado no sistema.";
                    break;

            }
            return View(nameof(StatusCode), viewModel);
        }
    }
}