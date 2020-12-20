using System.Collections.Generic;

namespace ProjectReferencesBuilder.Entities.Models
{
    public class Warning
    {
        public string ProjectName { get; init; }

        public List<string> Warnings { get; set; }
    }
}
