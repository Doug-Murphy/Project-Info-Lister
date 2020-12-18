using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder.Helpers
{
    public sealed class ProjectInfoService : IProjectInfoSetter
    {
        private bool _includeName;
        private bool _includeTfm;
        private bool _includeReferences;

        public static IProjectInfoSetter Start() => new ProjectInfoService();

        public IProjectInfoSetter WithName()
        {
            _includeName = true;

            return this;
        }

        public IProjectInfoSetter WithReferences()
        {
            _includeReferences = true;

            return this;
        }

        public IProjectInfoSetter WithTfm()
        {
            _includeTfm = true;

            return this;
        }

        public HashSet<ProjectInfo> GetInfo(string solutionFilePath)
        {
            if (FileHelper.GetFileExtension(solutionFilePath) != ".sln")
            {
                throw new ArgumentException("This file is not a .sln file.", nameof(solutionFilePath));
            }

            var fileContents = FileHelper.GetFileContents(solutionFilePath);
            var projectLineRegEx = new Regex(@"Project\(""\{.*\}""\).*""(.*)"",.*""(.*.csproj)", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var projectLineMatches = projectLineRegEx.Matches(string.Join("\n", fileContents));
            var projectsInSolution = new HashSet<ProjectInfo>();
            var projectInfoHelper = new ProjectInfoHelper(_includeName, _includeReferences, _includeTfm);

            for (int i = 0; i < projectLineMatches.Count; i++)
            {
                var fullyQualifiedPath = Path.GetFullPath(projectLineMatches[i].Groups[2].Value, FileHelper.GetFileDirectory(solutionFilePath));
                projectsInSolution.Add(new ProjectInfo(fullyQualifiedPath));
            }

            foreach (var projectInSolution in projectsInSolution)
            {
                projectInfoHelper.SetProjectInfo(projectInSolution);
            }

            return projectsInSolution;
        }
    }
}
