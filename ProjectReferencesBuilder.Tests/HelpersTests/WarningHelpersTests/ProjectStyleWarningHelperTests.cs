using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Tests.HelpersTests.WarningHelpersTests
{
    public class ProjectStyleWarningHelperTests
    {
        private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
        private static IEnumerable<TestCaseData> TestIsProjectUsingOldFormat_TestCases
        {
            get
            {
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { ProjectType = ProjectType.Pre2017Style }).Returns(true);
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { ProjectType = ProjectType.SDKStyle }).Returns(false);
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { ProjectType = null }).Returns(false);
            }
        }
        [TestCaseSource(nameof(TestIsProjectUsingOldFormat_TestCases))]
        public bool TestIsProjectUsingOldFormat(ProjectInfo project)
        {
            var mockProjectStyleWarningHelper = new ProjectStyleWarningHelper();
            var isOldFormat = mockProjectStyleWarningHelper.IsProjectUsingOldFormat(project);

            return isOldFormat;
        }
    }
}
