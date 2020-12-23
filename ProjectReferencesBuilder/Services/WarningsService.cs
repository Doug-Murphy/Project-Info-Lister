using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Factories;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReferencesBuilder.Services
{
    public class WarningsService : IWarningsService
    {
        private readonly IEndOfLifeWarningHelper _endOfLifeWarningHelper;
        private readonly IProjectStyleWarningHelper _projectStyleWarningHelper;

        public WarningsService(IEndOfLifeWarningHelper endOfLifeWarningHelper,
                               IProjectStyleWarningHelper projectStyleWarningHelper)
        {
            _endOfLifeWarningHelper = endOfLifeWarningHelper;
            _projectStyleWarningHelper = projectStyleWarningHelper;
        }

        public List<Warning> GetWarnings(IEnumerable<ProjectInfo> projects)
        {
            var warningsFound = new List<Warning>();
            var eolWarnings = new List<ProjectWarning>();
            var projectStyleWarnings = new List<ProjectWarning>();

            foreach (var project in projects)
            {
                if (_endOfLifeWarningHelper.IsProjectTfmEndOfLife(project, out string warningMessage))
                {
                    eolWarnings.Add(new ProjectWarning
                    {
                        ProjectName = project.Name,
                        Message = warningMessage
                    });
                }

                if (_projectStyleWarningHelper.IsProjectUsingOldFormat(project))
                {
                    projectStyleWarnings.Add(new ProjectWarning
                    {
                        ProjectName = project.Name,
                        Message = WarningMessageFactory.GetProjectStyleWarning()
                    });
                }

            }

            if (eolWarnings.Any())
            {
                warningsFound.AddRange(eolWarnings.Select(w => new Warning { WarningType = WarningType.EndOfLife, ProjectsAffected = eolWarnings }));
            }

            if (projectStyleWarnings.Any())
            {
                warningsFound.AddRange(projectStyleWarnings.Select(w => new Warning { WarningType = WarningType.ProjectStyle, ProjectsAffected = projectStyleWarnings }));
            }

            return warningsFound;
        }
    }
}
