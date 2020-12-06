using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProjectReferencesBuilder
{
    class Program
    {
        private static void SetProjectInfo(ProjectInfo projectToSetInfoFor)
        {
            SetProjectType(projectToSetInfoFor);
            SetProjectTFM(projectToSetInfoFor);
            SetProjectsReferencedByProject(projectToSetInfoFor);
        }

        private static void SetProjectTFM(ProjectInfo projectInfo)
        {
            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(projectInfo.AbsolutePath);

                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmldoc.NameTable);
                    mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                    foreach (XmlNode item in xmldoc.SelectNodes("Project/PropertyGroup/TargetFramework|Project/PropertyGroup/TargetFrameworks", mgr))
                    {
                        projectInfo.TFM = item.InnerXml;
                    }
                    break;
            }
        }

        private static void SetProjectsReferencedByProject(ProjectInfo projectInfo)
        {
            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:

                    break;
                default:
                    throw new NotSupportedException("How did you get here?");
            }

            projectInfo.ProjectsReferenced = null;
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

        private static void SetProjectType(ProjectInfo projectInfo)
        {
            if (FileHelper.GetFileExtension(projectInfo.AbsolutePath) != ".csproj")
            {
                throw new ArgumentException("This file is not a .csproj file.", nameof(projectInfo));
            }

            var fileContents = FileHelper.GetFileContents(projectInfo.AbsolutePath);

            if (fileContents.First() == "<Project Sdk=\"Microsoft.NET.Sdk\">")
            {
                projectInfo.ProjectType = ProjectType.SDKStyle;
            }
            else //TODO: Actually check first-line for non-SDK style projects and throw exception if we can't determine
            {
                projectInfo.ProjectType = ProjectType.Pre2017Style;
            }
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
