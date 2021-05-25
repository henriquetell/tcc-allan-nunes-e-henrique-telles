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
    public class GetNpsHttpTrigger : BaseTrigger
    {
        private readonly ILogger<GetNpsHttpTrigger> _log;
        private readonly ProdutoService _produtoService;
        private readonly ICloudStorage _cloudStorage;
        public GetNpsHttpTrigger(ILogger<GetNpsHttpTrigger> log, ProdutoService produtoService, ICloudStorage cloudStorage, IServiceProvider serviceProvider)
            : base(log, serviceProvider)
        {
            _log = log;
            _produtoService = produtoService;
            _cloudStorage = cloudStorage;
        }

        [FunctionName(nameof(GetNpsHttpTrigger))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "recuperar-nps/{idProduto:int?}/{id:guid?}")] HttpRequest req,
            int? idProduto,
            Guid? id)
        {
            if (!ValidateAuthorization(req))
                return new UnauthorizedResult();

            if (idProduto == null || id == null)
                return new UnauthorizedResult();
            try
            {
                var nps = _produtoService.RecuperarPorProdutoNps(id, idProduto);
                if (nps == null || (nps.DataLimite.HasValue && nps.DataLimite.Value.Date < DateTime.Now.Date))
                    return new UnauthorizedResult();

                nps.Imagem = _cloudStorage.RecuperarImagemUrl(nps.Imagem).AbsoluteUri;

                return new OkObjectResult(nps);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Ocorreru um erro");

                return new BadRequestResult();
            }
        }
    }
}
