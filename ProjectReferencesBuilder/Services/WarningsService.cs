using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReferencesBuilder.Services
{
    public class WarningsService : IWarningsService
    {
        private readonly IEndOfLifeWarningHelper _endOfLifeWarningHelper;

        public WarningsService(IEndOfLifeWarningHelper endOfLifeWarningHelper)
        {
            _endOfLifeWarningHelper = endOfLifeWarningHelper;
        }

        public List<Warning> GetWarnings(IEnumerable<ProjectInfo> projects)
        {
            var warningsFound = new List<Warning>();
            foreach (var project in projects)
            {
                var warningsForProject = new List<string>();

                if (_endOfLifeWarningHelper.IsProjectTfmEndOfLife(project, out string warningMessage)) {
                    warningsForProject.Add(warningMessage);
                }

                if (warningsForProject.Any())
                {
                    warningsFound.Add(new Warning
                    {
                        ProjectName = project.Name,
                        Warnings = warningsForProject
                    });
                }
            }

            return warningsFound;
        }
    }
}
