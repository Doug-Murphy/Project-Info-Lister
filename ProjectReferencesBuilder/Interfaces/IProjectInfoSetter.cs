using ProjectReferencesBuilder.Entities.Models;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Interfaces
{
    public interface IProjectInfoSetter
    {
        /// <summary>
        /// Specifies to include the name of the project in the information gathered.
        /// </summary>
        IProjectInfoSetter WithName();

        /// <summary>
        /// Specifies to include the references of the project in the information gathered.
        /// </summary>
        IProjectInfoSetter WithReferences();

        /// <summary>
        /// Specifies to include the TFM (target framework moniker) of the project in the information gathered.
        /// </summary>
        IProjectInfoSetter WithTfm();

        /// <summary>
        /// Execute the action to start the process.
        /// </summary>
        /// <param name="solutionFilePath">The absolute path to the .sln file</param>
        /// <returns></returns>
        HashSet<ProjectInfo> GetInfo(string solutionFilePath);
    }
}
