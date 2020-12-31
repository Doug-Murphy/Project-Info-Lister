using NUnit.Framework;
using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectReferencesBuilder.Tests.ServicesTests
{
    public class ProjectInfoServiceTests
    {
        private const string _pathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.sln";
        private readonly string _absolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(_pathToSampleProjectsSolution);

        [Test]
        public void TestGettingProjectName()
        {
            var projectInfo = new ProjectInfoService().WithName().GetInfo(_absolutePathToSampleProjectsSolution);

            foreach (var project in projectInfo)
            {
                Assert.That(project.Name, Is.Not.Null);
            }
        }

        [Test]
        public void TestGettingProjectReferences()
        {
            var projectInfo = new ProjectInfoService().WithName().WithReferences().GetInfo(_absolutePathToSampleProjectsSolution);

            foreach (var project in projectInfo)
            {
                switch (project.Name)
                {
                    case "ConsoleApp1":
                        CollectionAssert.IsNotEmpty(project.ProjectsReferenced);
                        Assert.That(project.ProjectsReferenced.Count, Is.EqualTo(1));
                        Assert.That(project.ProjectsReferenced.First().Name, Is.EqualTo("Services"));
                        break;
                    case "Entities":
                        CollectionAssert.IsNotEmpty(project.ProjectsReferenced);
                        Assert.That(project.ProjectsReferenced.Count, Is.EqualTo(1));
                        Assert.That(project.ProjectsReferenced.First().Name, Is.EqualTo("Entities.Interface"));
                        break;
                    case "Entities.Interface":
                        CollectionAssert.IsEmpty(project.ProjectsReferenced);
                        break;
                    case "OldStyle":
                        CollectionAssert.IsNotEmpty(project.ProjectsReferenced);
                        Assert.That(project.ProjectsReferenced.Count, Is.EqualTo(2));
                        Assert.That(project.ProjectsReferenced.First().Name, Is.EqualTo("Entities"));
                        break;
                    case "Services":
                        CollectionAssert.IsNotEmpty(project.ProjectsReferenced);
                        Assert.That(project.ProjectsReferenced.Count, Is.EqualTo(2));
                        Assert.That(project.ProjectsReferenced.First().Name, Is.EqualTo("Entities"));
                        break;
                    case "Services.Interface":
                        CollectionAssert.IsEmpty(project.ProjectsReferenced);
                        break;
                    case "EndOfLifeSample":
                        CollectionAssert.IsEmpty(project.ProjectsReferenced);
                        break;
                    default:
                        Assert.Fail(); //unexpected project found, fail to raise suspicion
                        break;
                }
            }
        }

        [Test]
        public void TestGettingProjectStyle()
        {
            var projectInfo = new ProjectInfoService().WithName().GetInfo(_absolutePathToSampleProjectsSolution);

            foreach (var project in projectInfo)
            {
                if (string.Equals(project.Name, "OldStyle", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.That(project.ProjectType, Is.EqualTo(ProjectType.Pre2017Style));
                }
                else
                {
                    Assert.That(project.ProjectType, Is.EqualTo(ProjectType.SDKStyle));
                }
            }
        }

        [Test]
        public void TestGettingProjectTfm()
        {
            var projectInfo = new ProjectInfoService().WithTfm().GetInfo(_absolutePathToSampleProjectsSolution);

            foreach (var project in projectInfo)
            {
                Assert.That(project.TFM, Is.Not.Null);
            }
        }

        [Test]
        public void TestSolutionNotEndingInSln()
        {
            string invalidPathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.notsln";
            string invalidAbsolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(invalidPathToSampleProjectsSolution);

            Assert.Throws<ArgumentException>(() => { var projectInfo = new ProjectInfoService().WithTfm().GetInfo(invalidAbsolutePathToSampleProjectsSolution); });
        }

        [Test]
        public void TestAddingTwoProjectInfoObjectsResultsInOne()
        {
            var projectInfo1 = new ProjectInfo("not_necessary");
            var projectInfo2 = new ProjectInfo("not_necessary");

            var projectsInSolution = new HashSet<ProjectInfo>(new ProjectInfoComparer());

            projectsInSolution.Add(projectInfo1);
            projectsInSolution.Add(projectInfo2);

            Assert.That(projectsInSolution.Count, Is.EqualTo(1));
        }
    }
}
