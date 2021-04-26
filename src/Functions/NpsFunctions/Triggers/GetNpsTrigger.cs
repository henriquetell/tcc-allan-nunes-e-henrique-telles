using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace NpsFunctions.Triggers
{
    public class GetNpsTrigger
    {
        private readonly ILogger<GetNpsTrigger> _log;
        private readonly ProdutoService _produtoService;
        private readonly ICloudStorage _cloudStorage;
        public GetNpsTrigger(ILogger<GetNpsTrigger> log, ProdutoService produtoService, ICloudStorage cloudStorage)
        {
            _log = log;
            _produtoService = produtoService;
            _cloudStorage = cloudStorage;
        }

        [FunctionName(nameof(GetNpsTrigger))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "recuperar-nps/produto/{idProduto:int?}/nps/{id:guid?}")] HttpRequest reg,
            int? idProduto,
            Guid? id)
        {
            if (idProduto == null || id == null)
                return new UnauthorizedResult();

            var nps = _produtoService.RecuperarPorProdutoNps(id, idProduto);
            if (nps == null || (nps.DataLimite.HasValue && nps.DataLimite.Value.Date < DateTime.Now.Date))
                return new UnauthorizedResult();

            nps.Imagem = _cloudStorage.RecuperarImagemUrl(nps.Imagem).AbsoluteUri;

            return new OkObjectResult(nps);
        }
    }
}
