using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGet;

namespace Router
{
    public static class ControllerFunction
    {
        [FunctionName("GetCurrentContext")]
        public static async Task<IActionResult> GetCurrentContextAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [DurableClient] IDurableEntityClient client,
            Microsoft.Extensions.Logging.ILogger log)
        {
            string app = req.Query["app"];
            if (string.IsNullOrEmpty(app))
            {
                return new BadRequestObjectResult("Please pass an app on the query string or in the request body");
            }
            // Check if there is Durable Entity Entry 
            var entity = await client.ReadEntityStateAsync<FunctionApp>(new EntityId(nameof(FunctionApp), app));
            var functionApp = entity.EntityState;

            return (ActionResult)new OkObjectResult(functionApp.GetCurrent());
        }

        // Version Check API 
        // It returns true/false if it requires new funtion app deployment
        [FunctionName("RequiredNewFunctionAppDeployment")]
        public static async Task<IActionResult> RequiredNewFunctionAppDeployment(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [DurableClient] IDurableEntityClient client,
        Microsoft.Extensions.Logging.ILogger log)
        {
            string app = req.Query["app"];
            string versionString = req.Query["version"];

            if (string.IsNullOrEmpty(app) || string.IsNullOrEmpty(versionString))
            {
                return new BadRequestObjectResult("Please pass an app on the query string or in the request body");
            } 

                // Check if there is Durable Entity 
                var entity = await client.ReadEntityStateAsync<FunctionApp>(new EntityId(nameof(FunctionApp), app));
                var context = entity.EntityState;
                
                var version = new SemanticVersion(versionString);
                var currentVersion = new SemanticVersion(context.Current);

                return new OkObjectResult(version.RequireNewDeployment(currentVersion));
        }

        [FunctionName("CreateOrUpdateAppContext")]
        public static async Task<IActionResult> CreateOrUpdateAppContextAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient client,
        Microsoft.Extensions.Logging.ILogger log)
        {
            var body = await req.ReadAsStringAsync();
            var appContext = JsonConvert.DeserializeObject<AppContext>(body);
            string instanceId = await client.StartNewAsync("CreateOrUpdateOrchestrator", null);
            return client.CreateCheckStatusResponse(req, instanceId);        
        }

        // TODO Create a timer trigger to check the running orchestration
        // If the runnning orchestrator is not there, update the status. 

    }
}
