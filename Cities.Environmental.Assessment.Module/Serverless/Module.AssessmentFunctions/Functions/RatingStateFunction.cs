using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Module.AssessmentFunctions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.AssessmentFunctions.Functions
{
    public static class RatingStateFunction
    {
        private static readonly string _projectId = "api-project-213531412446";

        [FunctionName(nameof(RatingStateFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RatingState")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing a request.");

            try
            {
                var response = new SurveyResponse();
                var db = FirestoreDb.Create(_projectId);
                var surveyRef = db.Collection("Survey");
                var query = surveyRef.WhereEqualTo("State", req.Query["state"].ToString().ToUpper());
                var querySnapshot = await query.GetSnapshotAsync();

                response.TotalResponses = querySnapshot.Documents.Count;

                if (response.TotalResponses > 0)
                {
                    foreach (var documentSnapshot in querySnapshot.Documents)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();

                        response.AvgQuality += city.Where(c => c.Key == nameof(SurveyCommand.Quality)).Select(c => (long)c.Value).FirstOrDefault();
                        response.AvgCrimes += city.Where(c => c.Key == nameof(SurveyCommand.Crimes)).Select(c => (long)c.Value).FirstOrDefault();
                        response.AvgPolitical += city.Where(c => c.Key == nameof(SurveyCommand.Political)).Select(c => (long)c.Value).FirstOrDefault();
                    }

                    response.AvgQuality /= response.TotalResponses;
                    response.AvgCrimes /= response.TotalResponses;
                    response.AvgPolitical /= response.TotalResponses;
                }

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Server Error");

                return new BadRequestObjectResult(new BadResponse("Ops, não foi possível buscar a informação!"));
            }
        }
    }
}
