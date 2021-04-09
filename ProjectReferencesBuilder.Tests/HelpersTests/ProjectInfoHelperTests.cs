using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;
using Xunit;

namespace ProjectReferencesBuilder.Tests.HelpersTests
{
    public class ProjectInfoHelperTests
    {
        [Fact]
        public void SetProjectInfoInvalidFileExtensionTest()
        {
            var projectWithInvalidFileExtension = new ProjectInfo("foo.notcsproj");
            var projectInfoHelper = new ProjectInfoHelper(false, false, false);

            Assert.Throws<ArgumentException>(() => { projectInfoHelper.SetProjectInfo(projectWithInvalidFileExtension); });
        }
    }
}