using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NpsFunctions.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NpsFunctions.Triggers
{
    public class PostNpsTrigger
    {
        private readonly ILogger<GetNpsTrigger> _log;
        private readonly ProdutoService _produtoService;
        public PostNpsTrigger(ILogger<GetNpsTrigger> log, ProdutoService produtoService)
        {
            _log = log;
            _produtoService = produtoService;

        }
        [FunctionName(nameof(PostNpsTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "salvar-nps/produto/{idProduto:int?}/nps/{id:guid?}")] HttpRequest req,
            int? idProduto,
            Guid? id)
        {
            if (idProduto == null || id == null)
                return new UnauthorizedResult();

            var nps = _produtoService.RecuperarPorProdutoNps(id, idProduto);
            if (nps == null || (nps.DataLimite.HasValue && nps.DataLimite.Value.Date < DateTime.Now.Date))
                return new UnauthorizedResult();

            using var request = new StreamReader(req.Body);
            var requestBody = await request.ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<SalvarNpsCommand>(requestBody);

            _produtoService.SalvarNps(id.Value, payload.Nota, payload.Comentario);

            return new OkObjectResult(null);
        }
    }
}
