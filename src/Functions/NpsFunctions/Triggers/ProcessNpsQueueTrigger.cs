using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace NpsFunctions.Triggers
{
    public class ProcessNpsQueueTrigger
    {
        private readonly ILogger<ProcessNpsQueueTrigger> _log;
        private readonly ProdutoService _produtoService;
        private readonly ICloudStorage _cloudStorage;

        public ProcessNpsQueueTrigger(ILogger<ProcessNpsQueueTrigger> log, ProdutoService produtoService, ICloudStorage cloudStorage)
        {
            _log = log;
            _produtoService = produtoService;
            _cloudStorage = cloudStorage;
        }

        [FunctionName(nameof(ProcessNpsQueueTrigger))]
        public void Run([QueueTrigger(CloudQueueNames.ProcessarNpsQueue, Connection = "AzureWebJobsStorage")] int idProduto)
        {
            try
            {
                _produtoService.ProcessarProdutoAvaliacaoNps(idProduto);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Não foi possível processar a função");
            }
        }
    }
}
