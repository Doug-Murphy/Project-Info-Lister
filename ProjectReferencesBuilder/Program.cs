using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder
{
    class Program
    {
        private static void SetProjectInfo(ProjectInfo projectToSetInfoFor)
        {
            projectToSetInfoFor.ProjectsReferenced = GetProjectsReferencedByProject(projectToSetInfoFor.AbsolutePath);
        }

        private static IEnumerable<ProjectInfo> GetProjectsReferencedByProject(string projectFilePath)
        {
            switch (GetProjectType(projectFilePath))
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:

                    break;
                default:
                    throw new NotSupportedException("How did you get here?");
            }

            return new List<ProjectInfo>();
        }

        private static IEnumerable<ProjectInfo> BuildDependencyDictionary(string solutionFilePath)
        {
            if (FileHelper.GetFileExtension(solutionFilePath) != ".sln")
            {
                throw new ArgumentException("This file is not a .sln file.", nameof(solutionFilePath));
            }

            var fileContents = FileHelper.GetFileContents(solutionFilePath);
            var projectLineRegEx = new Regex(@"Project\(""\{.*\}""\).*""(.*)"",.*""(.*.csproj)", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var projectLineMatches = projectLineRegEx.Matches(string.Join("\n", fileContents));
            
            var projectsInSolution = new HashSet<ProjectInfo>();
            for (int i = 0; i < projectLineMatches.Count; i++)
            {
                var fullyQualifiedPath = Path.GetFullPath(projectLineMatches[i].Groups[2].Value, FileHelper.GetFileDirectory(solutionFilePath));
                projectsInSolution.Add(new ProjectInfo { Name = projectLineMatches[i].Groups[1].Value, AbsolutePath = fullyQualifiedPath } );
            }

            foreach (var projectInSolution in projectsInSolution)
            {
                SetProjectInfo(projectInSolution);
            }

            return projectsInSolution;
        }

        private static ProjectType GetProjectType(string projectFilePath)
        {
            if (FileHelper.GetFileExtension(projectFilePath) != ".csproj")
            {
                throw new ArgumentException("This file is not a .csproj file.", nameof(projectFilePath));
            }

            var fileContents = FileHelper.GetFileContents(projectFilePath);

            if (fileContents.First() == "<Project Sdk=\"Microsoft.NET.Sdk\">")
            {
                return ProjectType.SDKStyle;
            }

            return ProjectType.Pre2017Style;
        }

        static void Main(string[] args)
        {
            string solutionFilePath;

            if (args.Length == 1)
            {
                solutionFilePath = args[0];
            }
            else
            {
                Console.Write("Enter the full path to the solution (.sln) file:");
                solutionFilePath = Console.ReadLine();
            }

            var projectsWithDependencies = BuildDependencyDictionary(solutionFilePath);
        }
    }
}
