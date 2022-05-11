using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace ProjectReferencesBuilder.Tests.ServicesTests;

[Collection("File-system access tests")]
public class ProjectInfoServiceTests
{
    private const string _pathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.sln";
    private readonly string _absolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(_pathToSampleProjectsSolution);

    [Fact]
    public void TestGettingProjectName()
    {
        var projectInfo = new ProjectInfoService().WithName().GetInfo(_absolutePathToSampleProjectsSolution);

        foreach (var project in projectInfo)
        {
            Assert.NotNull(project.Name);
        }
    }

    [Fact]
    public void TestGettingProjectReferences()
    {
        var projectInfo = new ProjectInfoService().WithName().WithReferences().GetInfo(_absolutePathToSampleProjectsSolution);

        foreach (var project in projectInfo)
        {
            switch (project.Name)
            {
                case "ConsoleApp1":
                    Assert.NotEmpty(project.ProjectsReferenced);
                    Assert.Single(project.ProjectsReferenced);
                    Assert.Equal("Services", project.ProjectsReferenced.First().Name);
                    break;
                case "Entities":
                    Assert.NotEmpty(project.ProjectsReferenced);
                    Assert.Single(project.ProjectsReferenced);
                    Assert.Equal("Entities.Interface", project.ProjectsReferenced.First().Name);
                    break;
                case "Entities.Interface":
                    Assert.Empty(project.ProjectsReferenced);
                    break;
                case "OldStyle":
                    Assert.NotEmpty(project.ProjectsReferenced);
                    Assert.Equal(2, project.ProjectsReferenced.Count);
                    Assert.Equal("Entities", project.ProjectsReferenced.First().Name);
                    break;
                case "Services":
                    Assert.NotEmpty(project.ProjectsReferenced);
                    Assert.Equal(2, project.ProjectsReferenced.Count);
                    Assert.Equal("Entities", project.ProjectsReferenced.First().Name);
                    break;
                case "Services.Interface":
                    Assert.Empty(project.ProjectsReferenced);
                    break;
                case "EndOfLifeSample":
                    Assert.Empty(project.ProjectsReferenced);
                    break;
                default:
                    throw new XunitException($"An unexpected project was found.");
            }
        }
    }

    [Fact]
    public void TestGettingProjectStyle()
    {
        var projectInfo = new ProjectInfoService().WithName().GetInfo(_absolutePathToSampleProjectsSolution);

        foreach (var project in projectInfo)
        {
            if (string.Equals(project.Name, "OldStyle", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Equal(ProjectType.Pre2017Style, project.ProjectType);
            }
            else
            {
                Assert.Equal(ProjectType.SDKStyle, project.ProjectType);
            }
        }
    }

    [Fact]
    public void TestGettingProjectTfm()
    {
        var projectInfo = new ProjectInfoService().WithTfm().GetInfo(_absolutePathToSampleProjectsSolution);

        foreach (var project in projectInfo)
        {
            Assert.NotNull(project.TFM);
        }
    }

    [Fact]
    public void TestSolutionNotEndingInSln()
    {
        string invalidPathToSampleProjectsSolution = "../../../../SampleProjects/SampleProjects.notsln";
        string invalidAbsolutePathToSampleProjectsSolution = PathHelper.GetAbsolutePath(invalidPathToSampleProjectsSolution);

        Assert.Throws<ArgumentException>(() => { var projectInfo = new ProjectInfoService().WithTfm().GetInfo(invalidAbsolutePathToSampleProjectsSolution); });
    }

    [Fact]
    public void TestAddingTwoProjectInfoObjectsResultsInOne()
    {
        var projectInfo1 = new ProjectInfo("not_necessary");
        var projectInfo2 = new ProjectInfo("not_necessary");

        var projectsInSolution = new HashSet<ProjectInfo>(new ProjectInfoComparer());

        projectsInSolution.Add(projectInfo1);
        projectsInSolution.Add(projectInfo2);

        Assert.Single(projectsInSolution);
    }
}