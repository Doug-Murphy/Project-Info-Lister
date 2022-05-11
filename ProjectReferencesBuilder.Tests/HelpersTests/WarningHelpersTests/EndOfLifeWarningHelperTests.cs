using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using System.Collections.Generic;
using Xunit;

namespace ProjectReferencesBuilder.Tests.HelpersTests.WarningHelpersTests;

public class EndOfLifeWarningHelperTests
{
    private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
    public static IEnumerable<object[]> TestIsProjectTfmEndOfLife_TestCases()
    {
        yield return new object[] { true, new ProjectInfo(_mockedAbsolutePath) { TFM = "netcoreapp2.2" } };
        yield return new object[] { true, new ProjectInfo(_mockedAbsolutePath) { TFM = "netcoreapp2.2;netcoreapp3.1" } };
        yield return new object[] { false, new ProjectInfo(_mockedAbsolutePath) { TFM = "net6.0" } };
    }

    [Theory]
    [MemberData(nameof(TestIsProjectTfmEndOfLife_TestCases))]
    public void TestIsProjectTfmEndOfLife(bool expectedResult, ProjectInfo project)
    {
        var mockEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
        var isEol = mockEndOfLifeWarningHelper.IsProjectTfmEndOfLife(project, out string _);

        Assert.Equal(expectedResult, isEol);
    }
}