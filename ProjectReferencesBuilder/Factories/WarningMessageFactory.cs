using ProjectReferencesBuilder.Entities.Models;
using System;

namespace ProjectReferencesBuilder.Factories
{
    public static class WarningMessageFactory
    {
        public static string GetEndOfLifeWarning(ProjectInfo project, DateTime eolDate)
        {
            return $"{project.TFM} hit end of life on {eolDate.ToShortDateString()}. Please consider upgrading to a LTS or newer target framework.";
        }

        public static string GetProjectStyleWarning()
        {
            return $"The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it.";
        }
    }
}
