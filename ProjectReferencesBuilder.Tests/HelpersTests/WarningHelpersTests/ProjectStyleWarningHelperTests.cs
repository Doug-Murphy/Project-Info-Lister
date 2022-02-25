using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using System.Collections.Generic;
using Xunit;

namespace ProjectReferencesBuilder.Tests.HelpersTests.WarningHelpersTests;

public class ProjectStyleWarningHelperTests
{
    private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
    public static IEnumerable<object[]> TestIsProjectUsingOldFormat_TestCases()
    {
        yield return new object[] { true, new ProjectInfo(_mockedAbsolutePath) { ProjectType = ProjectType.Pre2017Style } };
        yield return new object[] { false, new ProjectInfo(_mockedAbsolutePath) { ProjectType = ProjectType.SDKStyle } };
        yield return new object[] { false, new ProjectInfo(_mockedAbsolutePath) { ProjectType = null } };
    }

    [Theory]
    [MemberData(nameof(TestIsProjectUsingOldFormat_TestCases))]
    public void TestIsProjectUsingOldFormat(bool expectedResult, ProjectInfo project)
    {
        var mockProjectStyleWarningHelper = new ProjectStyleWarningHelper();
        var isOldFormat = mockProjectStyleWarningHelper.IsProjectUsingOldFormat(project);

        Assert.Equal(expectedResult, isOldFormat);
    }
}