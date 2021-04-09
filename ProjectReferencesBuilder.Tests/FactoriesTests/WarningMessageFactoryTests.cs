using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Factories;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProjectReferencesBuilder.Tests
{
    public class WarningMessageFactoryTests
    {
        private const string _mockedAbsolutePath = @"\\dummy\absolute\path.sln";
        public static IEnumerable<object[]> TestGetEndOfLifeWarning_TestCases()
        {
            var testProjectInfo = new ProjectInfo(_mockedAbsolutePath)
            {
                TFM = "netcoreapp2.2"
            };

            yield return new object[] { "netcoreapp2.2 hit end of life on January 02, 2020. Please consider upgrading to a LTS or newer target framework.", testProjectInfo, new DateTime(2020, 01, 02) };
        }
        [Theory]
        [MemberData(nameof(TestGetEndOfLifeWarning_TestCases))]
        public void TestGetEndOfLifeWarning(string expectedResult, ProjectInfo testProjectInfo, DateTime eolDate)
        {
            Assert.Equal(expectedResult, WarningMessageFactory.GetEndOfLifeWarning(testProjectInfo.TFM, eolDate));
        }

        [Fact]
        public void TestGetProjectStyleWarning()
        {
            Assert.Equal("The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it.", WarningMessageFactory.GetProjectStyleWarning());
        }
    }
}