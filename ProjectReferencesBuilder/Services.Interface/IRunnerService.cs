using ProjectReferencesBuilder.Entities.Models;

namespace ProjectReferencesBuilder.Services.Interface;

public interface IRunnerService
{
    ResultsOutput GetProjectDetails(string solutionFilePath);

    void WriteResultsToConsole(ResultsOutput results);

    void WriteResultsToFile(string solutionFilePath, ResultsOutput finalOutput);
}