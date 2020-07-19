using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Module.AssessmentFunctions.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Module.AssessmentFunctions.Functions
{
    public static class SurveyFunction
    {
        private static readonly string _projectId = "api-project-213531412446";

        [FunctionName(nameof(SurveyFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Survey")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Processing a request.");

                var db = FirestoreDb.Create(_projectId);
                using var sr = new StreamReader(req.Body);
                var requestBody = await sr.ReadToEndAsync();
                var body = Encoding.UTF8.GetString(Convert.FromBase64String(requestBody));
                var data = JsonConvert.DeserializeObject<SurveyCommand>(body);
                var docRef = db.Collection("Survey").Document(Guid.NewGuid().ToString());

                var user = new Dictionary<string, object>
                {
                    { nameof(SurveyCommand.Quality), data.Quality },
                    { nameof(SurveyCommand.Political), data.Political },
                    { nameof(SurveyCommand.Crimes), data.Crimes },
                    { nameof(SurveyCommand.City), data.City },
                    { nameof(SurveyCommand.State), data.State.ToUpper() },
                    { nameof(SurveyCommand.Name), data.Name },
                    { nameof(SurveyCommand.FreeResponse), data.FreeResponse },
                    { "Date", DateTime.UtcNow }
                };

                await docRef.SetAsync(user);

                log.LogInformation("Processed a request.");

                return new OkObjectResult(new SuccessResponse("Obrigado! Sua mensagem foi gravada com sucesso!"));
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Server Error");

                return new BadRequestObjectResult(new BadResponse("Ops, não foi possível gravar sua resposta!"));
            }
        }
    }
}
