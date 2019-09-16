using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SampleOrchestrator
{
    public static class SampleOrchestrator
    {
        [FunctionName("SampleOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {           
            await context.CallActivityAsync<string>("SampleOrchestrator_Hello", context.GetInput<string>());
        }

        [FunctionName("SampleOrchestrator_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("SampleOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [DurableClient]IDurableOrchestrationClient starter,
            ILogger log)
        {
            var functionAppName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("SampleOrchestrator", functionAppName + ":" + req.Content.ReadAsStringAsync());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}