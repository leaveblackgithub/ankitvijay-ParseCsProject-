using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace ParseCsProject
{
    public class Helpers
    {
        public static IList<string> GetProjectList(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath)) throw new ArgumentNullException(nameof(solutionPath));

            if (!Directory.Exists(solutionPath)) throw new DirectoryNotFoundException(solutionPath);

            IList<string> projectList = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories);
            foreach (var project in projectList) Debug.WriteLine(project);
            return projectList;
        }

        public static void RunWProPropList(IList<string> projectList, Action<string, ProjectPropertyGroupElement> action)
        {
            RunWProjList(projectList, proj =>
            {
                var projectPropertyGroupElements = proj.Xml.PropertyGroups;
                var projectName = Path.GetFileName(proj.FullPath);
                foreach (var projectPropertyGroupElement in projectPropertyGroupElements)
                    action(projectName, projectPropertyGroupElement);

            });
        }
        public static void RunWProjItemList(IList<string> projectList, Action<string, ProjectItemGroupElement> action)
        {
            RunWProjList(projectList, proj =>
            {
                var projectItemGroupElements = proj.Xml.ItemGroups;
                var projectName = Path.GetFileName(proj.FullPath);
                foreach (var projectItemGroupElement in projectItemGroupElements)
                    action(projectName, projectItemGroupElement);

            });
        }

        public static void RunWProjList(IList<string> projectList, Action<Project> action)
        {
            foreach (var project in projectList)
            {
                var projectCollection = new ProjectCollection();
                var proj = projectCollection.LoadProject(project);

                action(proj);
                proj.Save();
            }
        }

        public static Dictionary<string, List<ProjectPropertyElement>> NewProjPropertyDictionary(
            IList<string> projectList)
        {
            return projectList.ToDictionary(proPath => Path.GetFileName(proPath),
                proPath => new List<ProjectPropertyElement>());
        }

        public static Dictionary<string, List<ProjectItemElement>> NewProjItemDictionary(
            IList<string> projectList)
        {
            return projectList.ToDictionary(proPath => Path.GetFileName(proPath),
                proPath => new List<ProjectItemElement>());
        }

        public static Dictionary<string, List<ProjectPropertyElement>> GetProperties(IList<string> projectList,
            string name, string condition = "")
        {
            var dict =
                NewProjPropertyDictionary(projectList);
            RunWProPropList(projectList,
                (p, ge) => dict[p].AddRange(ge.Properties
                    .Where(prop => prop.Condition.Contains(condition) && prop.Name == name).ToList()));
            return dict;
        }

        public static Dictionary<string, List<ProjectItemElement>> GetItems(IList<string> projectList,
            string name, string include = "")
        {
            var dict =
                NewProjItemDictionary(projectList);
            RunWProjItemList(projectList,
                (p, ie) => dict[p].AddRange(ie.Items.Where(item => item.ElementName == name && item.Include.Contains(include)).ToList()));
            return dict;
        }

        public static void SetProperties(IList<string> projectList, string name, string value, string condition = "")
        {
            RunWProPropList(projectList, (proj, ge) =>
            {
                if(ge.Condition.Contains(condition))ge.SetProperty(name, value);
            });
        }
        public static void RemoveProperties(IList<string> projectList, string name,  string condition = "")
        {
            RunWProPropList(projectList, (proj, ge) =>
            {
                var props = ge.Properties.Where(prop => prop.Condition.Contains(condition) && prop.Name == name);
                foreach (var prop in props)
                {
                    ge.RemoveChild(prop);
                }
            });
        }
        public static void AddItems(IList<string> projectList, string itemType, string include )
        {

            RunWProjItemList(projectList, (proj, ie) =>
            {
                ie.AddItem(itemType, include);
            });
        }
        public static void RemoveItems(IList<string> projectList, string name, string include = "")
        {
            RunWProjItemList(projectList, (proj, ie) =>
            {
                var items = ie.Items.Where(item=>item.ElementName == name&&item.Include.Contains(include));
                foreach (var item in items)
                {
                    ie.RemoveChild(item);
                }
            });
        }
    }
}