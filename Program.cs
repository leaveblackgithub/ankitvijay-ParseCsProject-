using System.Diagnostics;

namespace ParseCsProject
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.Build.Construction;
    using Microsoft.Build.Evaluation;
    using Microsoft.Build.ObjectModelRemoting;

    public class Program
    {
        /// <summary>
        /// Remove <RootNamespace>,<AssemblyName>,<Deterministic>,<OutputPath>,AssemblyInfo.cs in all csproj
        /// </summary>
        public static void Main()
        {
            IList<string>  projectList = Helpers.GetProjectList(@"D:\leaveblackgithub\DDNCADAddinsForRevitImport\src");
            IList<string> propsToRemove = new List<string>()
                { "RootNamespace", "AssemblyName", "Deterministic", "OutputPath" };
            foreach (var propName in propsToRemove)
            {
                Helpers.RemoveProperties(projectList,propName);
            }
            Helpers.RemoveItems(projectList,Consts.ItemCompile,Consts.IncludeAssemblyinfoCs);
        }
        
    }
}