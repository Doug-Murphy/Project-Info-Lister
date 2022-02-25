using ProjectReferencesBuilder.Entities.Models;

namespace ProjectReferencesBuilder.Services.Interface;

public interface IWarningsService
{
    /// <summary>
    /// Obtain all warnings for all projects.
    /// </summary>
    /// <param name="projects"></param>
    /// <returns></returns>
    List<Warning> GetWarnings(IEnumerable<ProjectInfo> projects);
}