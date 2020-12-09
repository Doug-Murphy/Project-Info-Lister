using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace ProjectReferencesBuilder.Helpers
{
    public static class ProjectInfoHelper
    {
        public static void SetProjectInfo(ProjectInfo projectToSetInfoFor)
        {
            SetProjectName(projectToSetInfoFor);
            SetProjectType(projectToSetInfoFor);
            SetProjectTFM(projectToSetInfoFor);
            SetProjectsReferencedByProject(projectToSetInfoFor);
        }

        private static void SetProjectName(ProjectInfo projectToSetInfoFor)
        {
            projectToSetInfoFor.Name = Path.GetFileNameWithoutExtension(projectToSetInfoFor.AbsolutePath);
        }

        private static void SetProjectType(ProjectInfo projectInfo)
        {
            if (FileHelper.GetFileExtension(projectInfo.AbsolutePath) != ".csproj")
            {
                throw new ArgumentException("This file is not a .csproj file.", nameof(projectInfo));
            }

            var fileContents = FileHelper.GetFileContents(projectInfo.AbsolutePath);

            if (fileContents.First() == "<Project Sdk=\"Microsoft.NET.Sdk\">")
            {
                projectInfo.ProjectType = ProjectType.SDKStyle;
            }
            else //TODO: Actually check for non-SDK style projects and throw exception if we can't determine type
            {
                projectInfo.ProjectType = ProjectType.Pre2017Style;
            }
        }

        private static void SetProjectTFM(ProjectInfo projectInfo)
        {
            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:
                    var xmlDoc = ParseSdkStyleProject(projectInfo);
                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);

                    foreach (XmlNode item in xmlDoc.SelectNodes("Project/PropertyGroup/TargetFramework|Project/PropertyGroup/TargetFrameworks", mgr))
                    {
                        projectInfo.TFM = item.InnerXml;
                    }
                    break;
            }
        }

        private static void SetProjectsReferencedByProject(ProjectInfo projectInfo)
        {
            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    throw new NotImplementedException("Pre-2017 style csproj files are not yet supported.");
                    break;
                case ProjectType.SDKStyle:
                    var xmlDoc = ParseSdkStyleProject(projectInfo);
                    XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);

                    foreach (XmlNode item in xmlDoc.SelectNodes("Project/ItemGroup/ProjectReference", mgr))
                    {
                        var referencedProjectInfo = new ProjectInfo(Path.GetFullPath(item.Attributes["Include"].Value, FileHelper.GetFileDirectory(projectInfo.AbsolutePath)));
                        SetProjectInfo(referencedProjectInfo);
                        projectInfo.ProjectsReferenced.Add(referencedProjectInfo);
                    }
                    break;
                default:
                    throw new NotSupportedException("How did you get here?");
            }
        }

        private static XmlDocument ParseSdkStyleProject(ProjectInfo projectInfo)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(projectInfo.AbsolutePath);

            return xmldoc;
        }
    }
}
