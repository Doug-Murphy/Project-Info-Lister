using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Tests.HelpersTests.WarningHelpersTests
{
    [Parallelizable(ParallelScope.All)]
    public class EndOfLifeWarningHelperTests
    {
        private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
        private static IEnumerable<TestCaseData> TestIsProjectTfmEndOfLife_TestCases
        {
            get
            {
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { TFM = "netcoreapp2.2" }).Returns(true);
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { TFM = "netcoreapp2.2;netcoreapp3.1" }).Returns(true);
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { TFM = "net5.0" }).Returns(false);
                yield return new TestCaseData(new ProjectInfo(_mockedAbsolutePath) { TFM = "unsupported_TFM" }).Returns(false);
            }
        }
        [TestCaseSource(nameof(TestIsProjectTfmEndOfLife_TestCases))]
        public bool TestIsProjectTfmEndOfLife(ProjectInfo project)
        {
            var mockEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
            var isEol = mockEndOfLifeWarningHelper.IsProjectTfmEndOfLife(project, out string _);

            return isEol;
        }
    }
}
