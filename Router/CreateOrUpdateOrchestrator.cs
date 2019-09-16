using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Router
{
    public static class CreateOrUpdateOrchestrator
    {
        [FunctionName("CreateOrUpdateOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var appContext = context.GetInput<AppContext>();
            // Update the current Entity
            var functionApp = await context.CallEntityAsync<FunctionApp>(new EntityId(nameof(FunctionApp), appContext.Name), "CreateOrUpdate", appContext);
        }
    }
}