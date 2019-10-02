using NuGet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Router
{
    public static class SemanticVersionExtensions
    {
        /// <summary>
        /// Validate if new FunctionApp deployment is required. 
        /// </summary>
        /// <param name="version">Target SemanticVersion object</param>
        /// <param name="current">Current SemanticVersion object</param>
        /// <returns></returns>
        public static bool RequireNewDeployment(this SemanticVersion version, SemanticVersion current)
        {
            return (version.Version.Major > current.Version.Major) ||
                (version.Version.Major == current.Version.Major && version.Version.Minor > current.Version.Minor);
        }
    }
}
