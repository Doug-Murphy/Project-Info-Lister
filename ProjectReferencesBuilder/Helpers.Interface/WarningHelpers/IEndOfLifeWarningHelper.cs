using ProjectReferencesBuilder.Entities.Models;

namespace ProjectReferencesBuilder.Helpers.Interface.WarningHelpers
{
    public interface IEndOfLifeWarningHelper
    {
        /// <summary>
        /// Checks if the project's TFM is past end of life.
        /// </summary>
        /// <param name="project">The project to check the TFM for.</param>
        /// <param name="warningMessage">The warning message if the project is beyond EOL. Otherwise, null</param>
        /// <returns></returns>
        bool IsProjectTfmEndOfLife(ProjectInfo project, out string warningMessage);
    }
}
