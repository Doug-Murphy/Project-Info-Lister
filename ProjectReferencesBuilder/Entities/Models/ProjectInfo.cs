using ProjectReferencesBuilder.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ProjectReferencesBuilder.Entities.Models
{
    public class ProjectInfo
    {
        public ProjectInfo(string absolutePath)
        {
            AbsolutePath = absolutePath;
        }

        public string Name { get; set; }

        public string TFM { get; set; }

        public string AbsolutePath { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectType? ProjectType { get; set; }

        public List<ProjectInfo> ProjectsReferenced { get; set; } = new List<ProjectInfo>();
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
