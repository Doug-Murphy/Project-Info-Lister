using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using System.IO;
using System.Reflection;
using Xunit;

namespace ProjectReferencesBuilder.Tests.ServicesTests
{
    public class RunnerServiceTests
    {
        private const string _pathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.sln";
        private readonly string _absolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(_pathToSampleProjectsSolution);

        [Fact]
        public void TestWriteResultsToConsole()
        {
            var mockedEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
            var mockedProjectStyleWarningHelper = new ProjectStyleWarningHelper();
            var mockedProjectInfoService = new ProjectInfoService();
            var mockedWarningsService = new WarningsService(mockedEndOfLifeWarningHelper, mockedProjectStyleWarningHelper);
            var runnerService = new RunnerService(mockedProjectInfoService, mockedWarningsService);

            var results = runnerService.GetProjectDetails(_absolutePathToSampleProjectsSolution);

            runnerService.WriteResultsToConsole(results);
        }

        [Fact]
        public void TestWriteResultsToFile()
        {
            var mockedEndOfLifeWarningHelper = new EndOfLifeWarningHelper();
            var mockedProjectStyleWarningHelper = new ProjectStyleWarningHelper();
            var mockedProjectInfoService = new ProjectInfoService();
            var mockedWarningsService = new WarningsService(mockedEndOfLifeWarningHelper, mockedProjectStyleWarningHelper);
            var runnerService = new RunnerService(mockedProjectInfoService, mockedWarningsService);

            var results = runnerService.GetProjectDetails(_absolutePathToSampleProjectsSolution);
            var solutionFileName = FileHelper.GetFileName(_absolutePathToSampleProjectsSolution);

            runnerService.WriteResultsToFile(_absolutePathToSampleProjectsSolution, results);

            //cleanup written files
            var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var executingAssemblyDirectory = new DirectoryInfo(Path.GetDirectoryName(executingAssemblyPath));

            foreach (var sampleOutputFile in executingAssemblyDirectory.EnumerateFiles($"{solutionFileName}*.json")) {
                sampleOutputFile.Delete();
            }
        }
    }
}
