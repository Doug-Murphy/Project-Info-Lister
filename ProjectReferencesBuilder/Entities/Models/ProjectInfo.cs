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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AbsolutePath { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TFM { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectType? ProjectType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProjectInfo> ProjectsReferenced { get; set; }
    }

    public class ProjectInfoComparer : IEqualityComparer<ProjectInfo>
    {
        public bool Equals([DisallowNull] ProjectInfo x, [DisallowNull] ProjectInfo y)
        {
            return x.AbsolutePath == y.AbsolutePath;
        }

        public int GetHashCode([DisallowNull] ProjectInfo obj)
        {
            return obj.AbsolutePath.GetHashCode();
        }
    }
}
