using ProjectReferencesBuilder.Entities.Models;

namespace ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;

public interface IProjectStyleWarningHelper
{
    /// <summary>
    /// Returns true if the project is using the older style .csproj format
    /// </summary>
    /// <param name="project">The project to check the csproj format for.</param>
    /// <returns></returns>
    public bool IsProjectUsingOldFormat(ProjectInfo project);
}