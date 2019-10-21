using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Router
{
    public class RouterFunction
    {
        private readonly IHttpClientFactory _clientFactory;
        public RouterFunction(IHttpClientFactory factory)
        {
            _clientFactory = factory;
        }

        [FunctionName("StartOrchestration")]
        public async Task<IActionResult> StartOrchestrationAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [DurableClient] IDurableEntityClient entityClient,
        ILogger log)
        {
            var app = req.Query["app"];
            var functionName = req.Query["functionName"];
            // transfer the request to other functions
            var entity = await entityClient.ReadEntityStateAsync<FunctionApp>(new EntityId(nameof(FunctionApp), app));
            var functionApp = entity.EntityState;
            var currentFunctionApp = functionApp.GetCurrent();
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync($"https://{currentFunctionApp.FunctionAppName}.azurewebsites.net/api/{functionName}", new StreamContent(req.Body));
            if (response.IsSuccessStatusCode)
            {
                return new OkObjectResult(await response.Content.ReadAsStringAsync());
            } else
            {
                return new HttpResponseMessageResult(response);
            }
        }
    }
}
