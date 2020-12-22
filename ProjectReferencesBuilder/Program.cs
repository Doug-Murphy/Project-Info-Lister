using Microsoft.Extensions.DependencyInjection;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Interfaces;
using ProjectReferencesBuilder.Services;
using ProjectReferencesBuilder.Services.Interface;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder
{
    class Program
    {
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
 
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IEndOfLifeWarningHelper, EndOfLifeWarningHelper>()
                .AddSingleton<IProjectStyleWarningHelper, ProjectStyleWarningHelper>()
                .AddSingleton<IProjectInfoService, ProjectInfoService>()
                .AddSingleton<IWarningsService, WarningsService>()
                .BuildServiceProvider();

            var projectInfoService = serviceProvider.GetService<IProjectInfoService>();
            var warningsService = serviceProvider.GetService<IWarningsService>();

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

            var projectsWithInfo = projectInfoService.WithName().WithReferences().WithTfm().GetInfo(solutionFilePath);
            var warningsForProjects = warningsService.GetWarnings(projectsWithInfo);
            var finalOutput = new ResultsOutput
            {
                ProjectsWithInfo = projectsWithInfo,
                Warnings = warningsForProjects
            };

            //WriteResultsToConsole(finalOutput);
            WriteResultsToFile(solutionFilePath, finalOutput);
        }
    }
}
