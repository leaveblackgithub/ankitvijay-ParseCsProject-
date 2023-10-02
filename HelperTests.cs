using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ParseCsProject
{
    [TestFixture]
    public class HelperTests
    {
        private string PropOutputPath = "OutputPath";
        private IList<string> ProjectList { get; set; }

        [SetUp]
        public void Setup()
        {
            ProjectList = Helpers.GetProjectList(@"D:\leaveblackgithub\AutoCAD_UnitTest");
        }
        [Test]
        public void SetPropertiesTest()
        {
            
            Helpers.SetProperties(ProjectList, PropOutputPath, Consts.PathBuild);
            Assert.True(Helpers.GetProperties(ProjectList, PropOutputPath).Values
                .All(props => props.All(p => p.Value == Consts.PathBuild)));
            Helpers.SetProperties(ProjectList, PropOutputPath, Consts.PathDebug, Consts.CondDebug);
            Assert.True(Helpers.GetProperties(ProjectList, PropOutputPath, Consts.CondDebug).Values
                .All(props => props.All(p => p.Value == Consts.PathDebug)));
            Helpers.SetProperties(ProjectList, PropOutputPath, Consts.PathRelease, Consts.CondRelease);
            Assert.True(Helpers.GetProperties(ProjectList, PropOutputPath, Consts.CondRelease).Values
                .All(props => props.All(p => p.Value == Consts.PathRelease)));
        }

        [Test]
        public void RemovePropertiesTest()
        {
            Helpers.RemoveProperties(ProjectList, PropOutputPath);
            Assert.True(Helpers.GetProperties(ProjectList, PropOutputPath).Any());
        }

        [Test]
        public void GetItemTest()
        {
            var items = Helpers.GetItems(ProjectList, Consts.ItemCompile);
            Assert.True(items.Any());
        }

        [Test]
        public void AddItemsTest()
        {
            Helpers.AddItems(ProjectList, Consts.ItemCompile, Consts.IncludeAssemblyinfoCs);
            var items = Helpers.GetItems(ProjectList, Consts.ItemCompile, Consts.IncludeAssemblyinfoCs);
            Assert.True(items.Values.All(i=>i.Any()));
            // Helpers.RemoveItems(ProjectList, ItemCompile,IncludeAssemblyinfoCs);
            // items = Helpers.GetItems(ProjectList, ItemCompile, IncludeAssemblyinfoCs);
            // Assert.False(items.Any());
        }
        [Test]
        public void RemoveItemsTest()
        {
            // Helpers.AddItems(ProjectList, ItemCompile, IncludeAssemblyinfoCs);
            // var items = Helpers.GetItems(ProjectList, ItemCompile, IncludeAssemblyinfoCs);
            // Assert.True(items.Any());
            Helpers.RemoveItems(ProjectList, Consts.ItemCompile, Consts.IncludeAssemblyinfoCs);
            var items = Helpers.GetItems(ProjectList, Consts.ItemCompile, Consts.IncludeAssemblyinfoCs);
            Assert.False(items.Values.All(i => i.Any()));
        }
    }
}