using NUnit.Framework;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using System;
using System.IO;
using System.Reflection;

namespace ProjectReferencesBuilder.Tests.ServicesTests
{
    public class RunnerServiceTests
    {
        private const string _pathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.sln";
        private readonly string _absolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(_pathToSampleProjectsSolution);

        [Test]
        public void TestWriteResultsToConsole()
        {
            var mockedEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
            var mockedProjectStyleWarningHelper = new ProjectStyleWarningHelper();
            var mockedProjectInfoService = new ProjectInfoService();
            var mockedWarningsService = new WarningsService(mockedEndOfLifeWarningHelper, mockedProjectStyleWarningHelper);
            var runnerService = new RunnerService(mockedProjectInfoService, mockedWarningsService);

            var results = runnerService.GetProjectDetails(_absolutePathToSampleProjectsSolution);

            Assert.DoesNotThrow(() => { runnerService.WriteResultsToConsole(results); });
        }

        [Test]
        public void TestWriteResultsToFile()
        {
            var mockedEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
            var mockedProjectStyleWarningHelper = new ProjectStyleWarningHelper();
            var mockedProjectInfoService = new ProjectInfoService();
            var mockedWarningsService = new WarningsService(mockedEndOfLifeWarningHelper, mockedProjectStyleWarningHelper);
            var runnerService = new RunnerService(mockedProjectInfoService, mockedWarningsService);

            var results = runnerService.GetProjectDetails(_absolutePathToSampleProjectsSolution);
            var solutionFileName = FileHelper.GetFileName(_absolutePathToSampleProjectsSolution);

            Assert.DoesNotThrow(() => { runnerService.WriteResultsToFile(_absolutePathToSampleProjectsSolution, results); });

            //cleanup written files
            var executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new UriBuilder(executingAssemblyLocation);
            var executingAssemblyDirectory = new DirectoryInfo(Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)));

            foreach (var sampleOutputFile in executingAssemblyDirectory.EnumerateFiles($"{solutionFileName}*.json")) {
                sampleOutputFile.Delete();
            }
        }
    }
}
