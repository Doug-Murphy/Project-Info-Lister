using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ProjectReferencesBuilder.Helpers
{
    public sealed class ProjectInfoHelper
    {
        private readonly bool _includeName;
        private readonly bool _includeReferences;
        private readonly bool _includeTfm;

        public ProjectInfoHelper(bool includeName,
                                 bool includeReferences,
                                 bool includeTfm)
        {
            _includeName = includeName;
            _includeReferences = includeReferences;
            _includeTfm = includeTfm;
        }

        public void SetProjectInfo(ProjectInfo projectToSetInfoFor)
        {
            SetProjectType(projectToSetInfoFor);
            if (_includeName)
            {
                SetProjectName(projectToSetInfoFor);
            }
            if (_includeTfm)
            {
                SetProjectTFM(projectToSetInfoFor);
            }
            if (_includeReferences)
            {
                SetProjectsReferencedByProject(projectToSetInfoFor);
            }
        }

        private void SetProjectName(ProjectInfo projectToSetInfoFor)
        {
            projectToSetInfoFor.Name = Path.GetFileNameWithoutExtension(projectToSetInfoFor.AbsolutePath);
        }

        private void SetProjectType(ProjectInfo projectInfo)
        {
            if (FileHelper.GetFileExtension(projectInfo.AbsolutePath) != ".csproj")
            {
                throw new ArgumentException("This file is not a .csproj file.", nameof(projectInfo));
            }

            var fileContents = FileHelper.GetFileContents(projectInfo.AbsolutePath);

            if (fileContents.First().StartsWith("<Project Sdk="))
            {
                projectInfo.ProjectType = ProjectType.SDKStyle;
            }
            else //TODO: Actually check for non-SDK style projects and throw exception if we can't determine type
            {
                projectInfo.ProjectType = ProjectType.Pre2017Style;
            }
        }

        private void SetProjectTFM(ProjectInfo projectInfo)
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

        private void SetProjectsReferencedByProject(ProjectInfo projectInfo)
        {
            var projectFileXmlDocument = ParseProjectFileXml(projectInfo);
            XmlNamespaceManager xmlManager = new XmlNamespaceManager(projectFileXmlDocument.NameTable);
            projectInfo.ProjectsReferenced ??= new List<ProjectInfo>();

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

        private XmlDocument ParseProjectFileXml(ProjectInfo projectInfo)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(projectInfo.AbsolutePath);

            return xmldoc;
        }
    }
}
