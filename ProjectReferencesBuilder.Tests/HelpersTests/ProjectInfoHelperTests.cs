using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;

namespace ProjectReferencesBuilder.Tests.HelpersTests
{
    [Parallelizable(ParallelScope.All)]
    public class ProjectInfoHelperTests
    {
        [Test]
        public void SetProjectInfoInvalidFileExtensionTest()
        {
            var projectWithInvalidFileExtension = new ProjectInfo("foo.notcsproj");
            var projectInfoHelper = new ProjectInfoHelper(false, false, false);

            Assert.Throws<ArgumentException>(() => { projectInfoHelper.SetProjectInfo(projectWithInvalidFileExtension); });
        }
    }
}