using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    public class FunctionApp
    {
        [JsonProperty("contexts")]
        public IDictionary<string, AppContext> Contexts { get; set; }

        [JsonProperty("current")]
        public string Current { get; set; }

        public FunctionApp()
        {
            Contexts = new Dictionary<string, AppContext>();
        }

        public void CreateOrUpdate(AppContext context)
        {
            Current = context.Version.ToString();
            Contexts[context.Version.ToString()] = context;
        }
        public AppContext GetCurrent()
        {
           return Contexts.ContainsKey(Current) ? Contexts[Current] : null;
        }
        public IDictionary<string, AppContext> Get() => this.Contexts;

        [FunctionName(nameof(FunctionApp))]
        public static Task RunAsync([EntityTrigger] IDurableEntityContext ctx)
        => ctx.DispatchAsync<FunctionApp>();
    }
}
