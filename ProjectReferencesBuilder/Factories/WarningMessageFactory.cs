namespace ProjectReferencesBuilder.Factories;

public static class WarningMessageFactory
{
    public static string GetEndOfLifeWarning(string projectTfm, DateTime eolDate)
    {
        return $"{projectTfm} hit end of life on {eolDate:MMMM dd, yyyy}. Please consider upgrading to a LTS or newer target framework.";
    }

    public static string GetProjectStyleWarning()
    {
        return $"The project style for this project is outdated and has many drawbacks compared to the SDK-style csproj. Please consider upgrading it.";
    }
}