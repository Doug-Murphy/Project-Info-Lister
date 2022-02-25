using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Factories;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using DougMurphy.TargetFrameworks.EndOfLife.Helpers;

namespace ProjectReferencesBuilder.Helpers.WarningHelpers;

public class EndOfLifeWarningHelper : IEndOfLifeWarningHelper
{
    public bool IsProjectTfmEndOfLife(ProjectInfo project, out string warningMessage)
    {
        List<string> eolWarnings = new List<string>();
        var eolCheck = TargetFrameworkEndOfLifeHelper.CheckTargetFrameworkForEndOfLife(project.TFM);
        foreach (var (tfm, eolDate) in eolCheck.EndOfLifeTargetFrameworks)
        {
            eolWarnings.Add(WarningMessageFactory.GetEndOfLifeWarning(tfm, eolDate));
        }

        warningMessage = eolWarnings.Any() ? string.Join(" ", eolWarnings) : null;

        return !string.IsNullOrWhiteSpace(warningMessage);
    }
}