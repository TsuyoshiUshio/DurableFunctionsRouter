using NuGet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Router
{
    /// <summary>
    /// Context object for Function App Version. 
    /// It is stored on the function app. 
    /// </summary>
    public class AppContext
    {
        /// <summary>
        /// Name of an Application. 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Name of ResourceGroup
        /// </summary>
        public string ResourceGroup { get; set; }
        /// <summary>
        /// Region
        /// <see cref="Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region">Region</see>
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Function App name. 
        /// </summary>
        public string FunctionAppName { get; set; }
        public string Location { get; set; }
        /// <summary>
        /// Application Version
        /// </summary>
        public SemanticVersion Version { get; set; }

        /// <summary>
        /// State of the Function App
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Update object if there is an non-null parameters except for State. 
        /// </summary>
        /// <param name=""></param>
        public void Update(AppContext context)
        {
            Name = context.Name ?? Name;
            ResourceGroup = context.ResourceGroup ?? ResourceGroup;
            Region = context.Region ?? Region;
            FunctionAppName = context.FunctionAppName ?? FunctionAppName;
            Version = context.Version ?? Version;
        }


    }

}
