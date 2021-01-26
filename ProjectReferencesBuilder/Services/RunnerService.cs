using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Services.Interface;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder.Services
{
    public class RunnerService : IRunnerService
    {
        private readonly IProjectInfoService _projectInfoService;
        private readonly IWarningsService _warningsService;

        public RunnerService(IProjectInfoService projectInfoService,
                             IWarningsService warningsService)
        {
            _projectInfoService = projectInfoService;
            _warningsService = warningsService;
        }

        public ResultsOutput GetProjectDetails(string solutionFilePath)
        {
            var projectsWithInfo = _projectInfoService.WithName().WithReferences().WithTfm().GetInfo(solutionFilePath);
            var warningsForProjects = _warningsService.GetWarnings(projectsWithInfo);
            var finalOutput = new ResultsOutput
            {
                ProjectsWithInfo = projectsWithInfo,
                Warnings = warningsForProjects
            };

            return finalOutput;
        }

        public void WriteResultsToConsole(ResultsOutput results)
        {
            Console.WriteLine(JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void WriteResultsToFile(string solutionFilePath, ResultsOutput finalOutput)
        {
            var solutionName = FileHelper.GetFileName(solutionFilePath);
            var fileName = $"{solutionName}_{DateTime.Now}";
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            var regExReplacer = new Regex($"[{Regex.Escape(invalidChars)}]");
            fileName = regExReplacer.Replace(fileName, "_") + ".json";
            File.WriteAllText(fileName, JsonSerializer.Serialize(finalOutput, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
