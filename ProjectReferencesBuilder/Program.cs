using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ProjectReferencesBuilder
{
    class Program
    {
        private static void PrintResults(IEnumerable<ProjectInfo> consolidatedData)
        {
            Console.WriteLine(JsonSerializer.Serialize(consolidatedData, new JsonSerializerOptions { WriteIndented = true }));
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
            var projectsWithDependencies = new ProjectInfoService().BuildProjectInfo(solutionFilePath);

            PrintResults(projectsWithDependencies);
        }
    }
}
