using ProjectReferencesBuilder.Entities.Enums;
using ProjectReferencesBuilder.Entities.Models;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProjectReferencesBuilder.Tests.ServicesTests;

public class WarningsServiceTests
{
    [Fact]
    public void TestGetWarnings()
    {
        var eolProjectInfo = new ProjectInfo("do_not_need_this_for_this_test")
        {
            Name = "TestWarningsProject",
            ProjectType = ProjectType.Pre2017Style,
            TFM = "netcoreapp2.2",
        };

        var warningsService = new WarningsService(new EndOfLifeWarningHelper(), new ProjectStyleWarningHelper());

        var warningsFound = warningsService.GetWarnings(new List<ProjectInfo> { eolProjectInfo });

        Assert.NotEmpty(warningsFound);
        Assert.All(warningsFound, (warningFound) => { Assert.IsType<Warning>(warningFound); });
        var eolWarningFound = warningsFound.FirstOrDefault(x => x.WarningType == WarningType.EndOfLife);
        var projectStyleWarningFound = warningsFound.FirstOrDefault(x => x.WarningType == WarningType.ProjectStyle);

        Assert.Equal(WarningType.EndOfLife, eolWarningFound.WarningType);
        Assert.Equal("netcoreapp2.2 hit end of life on December 23, 2019. Please consider upgrading to a LTS or newer target framework.", eolWarningFound.ProjectsAffected.First().Message);
        Assert.Equal("TestWarningsProject", eolWarningFound.ProjectsAffected.First().ProjectName);

        Assert.Equal(WarningType.ProjectStyle, projectStyleWarningFound.WarningType);
        Assert.Equal("The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it.", projectStyleWarningFound.ProjectsAffected.First().Message);
        Assert.Equal("TestWarningsProject", projectStyleWarningFound.ProjectsAffected.First().ProjectName);
    }
}