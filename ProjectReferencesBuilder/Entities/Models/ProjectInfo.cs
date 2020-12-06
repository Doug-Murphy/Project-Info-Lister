using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectReferencesBuilder.Entities.Models
{
    public class ProjectInfo
    {
        public string Name { get; set; }

        public string TFM { get; set; }

        public string AbsolutePath { get; set; }

        public IEnumerable<ProjectInfo> ProjectsReferenced { get; set; }
    }

    public class ProjectInfoComparer : IEqualityComparer<ProjectInfo>
    {
        public bool Equals([DisallowNull] ProjectInfo x, [DisallowNull] ProjectInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] ProjectInfo obj)
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.Name);
        }
    }
}
