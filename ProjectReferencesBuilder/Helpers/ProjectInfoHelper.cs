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
            var projectFileXmlDocument = ParseProjectFileXml(projectInfo);
            XmlNamespaceManager xmlManager = new XmlNamespaceManager(projectFileXmlDocument.NameTable);

            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    xmlManager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                    foreach (XmlNode item in projectFileXmlDocument.SelectNodes("//x:TargetFrameworkVersion", xmlManager))
                    {
                        projectInfo.TFM = item.InnerXml;
                    }
                    break;
                case ProjectType.SDKStyle:
                    foreach (XmlNode item in projectFileXmlDocument.SelectNodes("Project/PropertyGroup/TargetFramework|Project/PropertyGroup/TargetFrameworks", xmlManager))
                    {
                        projectInfo.TFM = item.InnerXml;
                    }
                    break;
                default:
                    throw new NotSupportedException("How did you get here?");
            }
        }

        private static void SetProjectsReferencedByProject(ProjectInfo projectInfo)
        {
            var projectFileXmlDocument = ParseProjectFileXml(projectInfo);
            XmlNamespaceManager xmlManager = new XmlNamespaceManager(projectFileXmlDocument.NameTable);

            switch (projectInfo.ProjectType)
            {
                case ProjectType.Pre2017Style:
                    xmlManager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                    foreach (XmlNode item in projectFileXmlDocument.SelectNodes("//x:ProjectReference", xmlManager))
                    {
                        var referencedProjectInfo = new ProjectInfo(Path.GetFullPath(item.Attributes["Include"].Value, FileHelper.GetFileDirectory(projectInfo.AbsolutePath)));
                        SetProjectInfo(referencedProjectInfo);
                        projectInfo.ProjectsReferenced.Add(referencedProjectInfo);
                    }
                    break;
                case ProjectType.SDKStyle:
                    foreach (XmlNode item in projectFileXmlDocument.SelectNodes("Project/ItemGroup/ProjectReference", xmlManager))
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

        private static XmlDocument ParseProjectFileXml(ProjectInfo projectInfo)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(projectInfo.AbsolutePath);

            return xmldoc;
        }
    }
}
