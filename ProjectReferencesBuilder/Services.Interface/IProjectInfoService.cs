using ProjectReferencesBuilder.Entities.Models;

namespace ProjectReferencesBuilder.Services.Interface;

public interface IProjectInfoService
{
    /// <summary>
    /// Specifies to include the name of the project in the information gathered.
    /// </summary>
    IProjectInfoService WithName();

    /// <summary>
    /// Specifies to include the references of the project in the information gathered.
    /// </summary>
    IProjectInfoService WithReferences();

    /// <summary>
    /// Specifies to include the TFM (target framework moniker) of the project in the information gathered.
    /// </summary>
    IProjectInfoService WithTfm();

    /// <summary>
    /// Execute the action to start the process.
    /// </summary>
    /// <param name="solutionFilePath">The absolute path to the .sln file</param>
    /// <returns></returns>
    HashSet<ProjectInfo> GetInfo(string solutionFilePath);
}