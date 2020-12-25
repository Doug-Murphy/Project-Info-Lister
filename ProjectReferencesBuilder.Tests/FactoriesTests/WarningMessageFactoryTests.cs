using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Factories;
using System;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Tests
{
    [Parallelizable(ParallelScope.All)]
    public class WarningMessageFactoryTests
    {
        private static IEnumerable<TestCaseData> TestGetEndOfLifeWarning_TestCases
        {
            get
            {
                var testProjectInfo = new ProjectInfo(@"\\dummy\absolute\path")
                {
                    TFM = "netcoreapp2.2"
                };

                yield return new TestCaseData(testProjectInfo, new DateTime(2020, 01, 02)).Returns("netcoreapp2.2 hit end of life on 1/2/2020. Please consider upgrading to a LTS or newer target framework.");
            }
        }
        [TestCaseSource(nameof(TestGetEndOfLifeWarning_TestCases))]
        public string TestGetEndOfLifeWarning(ProjectInfo testProjectInfo, DateTime eolDate)
        {
            return WarningMessageFactory.GetEndOfLifeWarning(testProjectInfo, eolDate);
        }

        [Test]
        public void TestGetProjectStyleWarning()
        {
            Assert.That(WarningMessageFactory.GetProjectStyleWarning(), Is.EqualTo("The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it."));
        }
    }
}