using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProjectReferencesBuilder
{
    class Program
    {
        //TODO: Don't use global vars! Probably want to use an instanced class or something anyway.
        private static string _solutionFilePath;
        private static HashSet<ProjectInfo> _projectsInSolution = new HashSet<ProjectInfo>();

        private static XmlDocument ParseSdkStyleProject(ProjectInfo projectInfo)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(projectInfo.AbsolutePath);

            return xmldoc;
        }

        private static void SetProjectInfo(ProjectInfo projectToSetInfoFor)
        {
            SetProjectName(projectToSetInfoFor);
            SetProjectType(projectToSetInfoFor);
            SetProjectTFM(projectToSetInfoFor);
            SetProjectsReferencedByProject(projectToSetInfoFor);
        }

        private static void SetProjectName(ProjectInfo projectToSetInfoFor)
        {
            projectToSetInfoFor.Name = _projectsInSolution.FirstOrDefault(x => x.AbsolutePath == projectToSetInfoFor.AbsolutePath)?.Name;
        }

        private static void SetProjectTFM(ProjectInfo projectInfo)
        {
            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:
                    var xmlDoc = ParseSdkStyleProject(projectInfo);
                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);

                    foreach (XmlNode item in xmlDoc.SelectNodes("Project/PropertyGroup/TargetFramework|Project/PropertyGroup/TargetFrameworks", mgr))
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
                    var xmlDoc = ParseSdkStyleProject(projectInfo);
                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);

                    foreach (XmlNode item in xmlDoc.SelectNodes("Project/ItemGroup/ProjectReference", mgr))
                    {
                        var referencedProjectInfo = new ProjectInfo { AbsolutePath = Path.GetFullPath(item.Attributes["Include"].Value, FileHelper.GetFileDirectory(projectInfo.AbsolutePath)) };
                        SetProjectInfo(referencedProjectInfo);
                        projectInfo.ProjectsReferenced.Add(referencedProjectInfo);
                    }
                    break;
                default:
                    throw new NotSupportedException("How did you get here?");
            }
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
            
            for (int i = 0; i < projectLineMatches.Count; i++)
            {
                var fullyQualifiedPath = Path.GetFullPath(projectLineMatches[i].Groups[2].Value, FileHelper.GetFileDirectory(solutionFilePath));
                _projectsInSolution.Add(new ProjectInfo { Name = projectLineMatches[i].Groups[1].Value, AbsolutePath = fullyQualifiedPath } );
            }

            foreach (var projectInSolution in _projectsInSolution)
            {
                SetProjectInfo(projectInSolution);
            }

            return _projectsInSolution;
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
            else //TODO: Actually check for non-SDK style projects and throw exception if we can't determine type
            {
                projectInfo.ProjectType = ProjectType.Pre2017Style;
            }
        }

        private static void PrintResults(IEnumerable<ProjectInfo> consolidatedData)
        {
            Console.WriteLine(JsonSerializer.Serialize(consolidatedData, new JsonSerializerOptions { WriteIndented = true }));
        }

        static void Main(string[] args)
        {

            if (args.Length == 1)
            {
                _solutionFilePath = args[0];
            }
            else
            {
                Console.Write("Enter the full path to the solution (.sln) file:");
                _solutionFilePath = Console.ReadLine();
            }

            var projectsWithDependencies = BuildDependencyDictionary(_solutionFilePath);

            PrintResults(projectsWithDependencies);
        }
    }
}
