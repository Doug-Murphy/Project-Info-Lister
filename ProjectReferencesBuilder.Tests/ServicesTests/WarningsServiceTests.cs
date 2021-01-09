using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReferencesBuilder.Tests.ServicesTests
{
    [Parallelizable(ParallelScope.All)]
    public class WarningsServiceTests
    {
        [Test]
        public void TestGetWarnings()
        {
            var eolProjectInfo = new ProjectInfo("do_not_need_this_for_this_test")
            {
                Name = "TestWarningsProject",
                ProjectType = ProjectType.Pre2017Style,
                TFM = "netcoreapp2.2",
            };

            var warningsService = new WarningsService(new EndOfLifeWarningHelper(), new ProjectStyleWarningHelper());

            var warningsFound = warningsService.GetWarnings(new List<ProjectInfo> { eolProjectInfo });

            CollectionAssert.IsNotEmpty(warningsFound);
            CollectionAssert.AllItemsAreInstancesOfType(warningsFound, typeof(Warning));
            var eolWarningFound = warningsFound.FirstOrDefault(x => x.WarningType == WarningType.EndOfLife);
            var projectStyleWarningFound = warningsFound.FirstOrDefault(x => x.WarningType == WarningType.ProjectStyle);

            Assert.That(eolWarningFound.WarningType, Is.EqualTo(WarningType.EndOfLife));
            Assert.That(eolWarningFound.ProjectsAffected.First().Message, Is.EqualTo("netcoreapp2.2 hit end of life on December 23, 2019. Please consider upgrading to a LTS or newer target framework."));
            Assert.That(eolWarningFound.ProjectsAffected.First().ProjectName, Is.EqualTo("TestWarningsProject"));

            Assert.That(projectStyleWarningFound.WarningType, Is.EqualTo(WarningType.ProjectStyle));
            Assert.That(projectStyleWarningFound.ProjectsAffected.First().Message, Is.EqualTo("The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it."));
            Assert.That(projectStyleWarningFound.ProjectsAffected.First().ProjectName, Is.EqualTo("TestWarningsProject"));
        }
    }
}
