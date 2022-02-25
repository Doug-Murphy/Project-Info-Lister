namespace ProjectReferencesBuilder.Helpers;

public static class FileHelper
{
    private static readonly Dictionary<string, IEnumerable<string>> _cachedFileContents = new Dictionary<string, IEnumerable<string>>();

    public static IEnumerable<string> GetFileContents(string filePath)
    {
        if (_cachedFileContents.ContainsKey(filePath))
        {
            return _cachedFileContents[filePath];
        }

        var fileContents = File.ReadAllLines(filePath);
        if (fileContents.Length == 0)
        {
            throw new InvalidOperationException("The project file path is an empty file.");
        }

        _cachedFileContents.Add(filePath, fileContents);

        return fileContents;
    }

    public static string GetFileExtension(string filePath)
    {
        return Path.GetExtension(filePath?.Trim('"'));
    }

    public static string GetFileDirectory(string filePath)
    {
        return Path.GetDirectoryName(filePath);
    }

    public static string GetFileName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }
}