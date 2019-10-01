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
        [FunctionName("GetTargetFunctionApp")]
        public static async Task<IActionResult> GetTargetFunctionAppAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient client,
        Microsoft.Extensions.Logging.ILogger log)
        {
            var body = await req.ReadAsStringAsync();
            var appContext = JsonConvert.DeserializeObject<AppContext>(body);

            if (appContext == null || appContext.Version == null)
            {
                return new BadRequestObjectResult("Malformed Json format or missing Version. Check the value");
            }
              
            string instanceId = await client.StartNewAsync("CreateOrUpdateFunctionAppOrchestrator", appContext);
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
