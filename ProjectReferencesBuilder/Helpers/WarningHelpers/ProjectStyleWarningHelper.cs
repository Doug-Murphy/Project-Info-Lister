using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;

namespace ProjectReferencesBuilder.Helpers.WarningHelpers
{
    public class ProjectStyleWarningHelper : IProjectStyleWarningHelper
    {
        public bool IsProjectUsingOldFormat(ProjectInfo project)
        {
            return project.ProjectType == ProjectType.Pre2017Style;
        }
    }
}
