using ProjectReferencesBuilder.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectReferencesBuilder.Helpers
{
    public class ProjectInfoService
    {
        private readonly HashSet<ProjectInfo> _projectsInSolution = new HashSet<ProjectInfo>();

        public IEnumerable<ProjectInfo> BuildProjectInfo(string solutionFilePath)
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
                _projectsInSolution.Add(new ProjectInfo(fullyQualifiedPath));
            }

            foreach (var projectInSolution in _projectsInSolution)
            {
                ProjectInfoHelper.SetProjectInfo(projectInSolution);
            }

            return _projectsInSolution;
        }
    }
}
