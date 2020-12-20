using System.Collections.Generic;

namespace ProjectReferencesBuilder.Entities.Models
{
    public class ResultsOutput
    {
        public HashSet<ProjectInfo> ProjectsWithInfo { get; init; }

        public List<Warning> Warnings { get; set; }
    }
}
