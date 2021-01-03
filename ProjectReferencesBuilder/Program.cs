using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using ProjectReferencesBuilder.Services.Interface;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder
{
    public class Program
    {
        private readonly IProjectInfoService _projectInfoService;
        private readonly IWarningsService _warningsService;

        public Program(IProjectInfoService projectInfoService,
                       IWarningsService warningsService)
        {
            _projectInfoService = projectInfoService;
            _warningsService = warningsService;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Program>();
                    services.AddSingleton<IEndOfLifeWarningHelper, EndOfLifeWarningHelper>();
                    services.AddSingleton<IProjectStyleWarningHelper, ProjectStyleWarningHelper>();
                    services.AddSingleton<IProjectInfoService, ProjectInfoService>();
                    services.AddSingleton<IWarningsService, WarningsService>();
                });
        }
 
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Services.GetRequiredService<Program>().Run(args);
        }

        public void Run(string[] args)
        {
            string solutionFilePath = "";
            if (args.Length == 1)
            {
                solutionFilePath = args[0];
            }

            while (FileHelper.GetFileExtension(solutionFilePath) != ".sln")
            {
                Console.Write("Enter the full path to the solution (.sln) file: ");
                solutionFilePath = Console.ReadLine();
            }

            solutionFilePath = solutionFilePath.Trim('"');

            var projectsWithInfo = _projectInfoService.WithName().WithReferences().WithTfm().GetInfo(solutionFilePath);
            var warningsForProjects = _warningsService.GetWarnings(projectsWithInfo);
            var finalOutput = new ResultsOutput
            {
                ProjectsWithInfo = projectsWithInfo,
                Warnings = warningsForProjects
            };

            WriteResultsToConsole(finalOutput);
            //WriteResultsToFile(solutionFilePath, finalOutput);
        }

        private static void WriteResultsToConsole(ResultsOutput results)
        {
            Console.WriteLine(JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
        }

        private static void WriteResultsToFile(string solutionFilePath, ResultsOutput finalOutput)
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
