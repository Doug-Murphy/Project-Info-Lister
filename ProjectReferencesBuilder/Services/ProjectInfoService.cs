using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder.Services
{
    public sealed class ProjectInfoService : IProjectInfoService
    {
        private bool _includeName;
        private bool _includeTfm;
        private bool _includeReferences;

        public IProjectInfoService WithName()
        {
            _includeName = true;

            return this;
        }

        public IProjectInfoService WithReferences()
        {
            _includeReferences = true;

            return this;
        }

        public IProjectInfoService WithTfm()
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
                var fullyQualifiedPath = PathHelper.GetAbsolutePath(projectLineMatches[i].Groups[2].Value, FileHelper.GetFileDirectory(solutionFilePath));
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
