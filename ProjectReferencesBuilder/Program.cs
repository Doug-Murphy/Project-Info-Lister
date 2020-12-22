using Microsoft.Extensions.DependencyInjection;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Interfaces;
using ProjectReferencesBuilder.Services;
using ProjectReferencesBuilder.Services.Interface;
using System;
using System.Text.Json;

namespace ProjectReferencesBuilder
{
    class Program
    {
        private static void PrintResults(ResultsOutput results)
        {
            Console.WriteLine(JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
        }

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IEndOfLifeWarningHelper, EndOfLifeWarningHelper>()
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

            PrintResults(finalOutput);
        }
    }
}
