using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Services;
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

            var projectsWithInfo = ProjectInfoService.Start().WithName().WithReferences().WithTfm().GetInfo(solutionFilePath);
            var warningsForProjects = new WarningsService().GetWarnings(projectsWithInfo);
            var finalOutput = new ResultsOutput
            {
                ProjectsWithInfo = projectsWithInfo,
                Warnings = warningsForProjects
            };

            PrintResults(finalOutput);
        }
    }
}
