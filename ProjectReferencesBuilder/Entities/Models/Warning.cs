using ProjectReferencesBuilder.Entities.Enums;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Entities.Models
{
    public class Warning
    {
        public WarningType WarningType { get; init; }

        public List<ProjectWarning> ProjectsAffected { get; init; }
    }
}
