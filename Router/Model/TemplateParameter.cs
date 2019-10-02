using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Router
{
    public class StringParameter
    {
        public string value { get; set; }
        public StringParameter(string value)
        {
            this.value = value;
        }
    }

    public class TemplateParameter
    {
        [JsonProperty(PropertyName = "appName")]
        public StringParameter AppName { get; set; }

        [JsonProperty(PropertyName = "runtime")]
        public StringParameter Runtime { get; set; }

        public TemplateParameter(string appName)
        {
            AppName = new StringParameter(appName);
            Runtime = new StringParameter("dotnet");
        }
    }
}
