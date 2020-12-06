using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectReferencesBuilder.Helpers
{
    public static class FileHelper
    {
        public static IEnumerable<string> GetFileContents(string filePath)
        {
            var fileContents = File.ReadAllLines(filePath);
            if (fileContents == null || fileContents.Length == 0)
            {
                throw new InvalidOperationException("The project file path is an empty file.");
            }

            return fileContents;
        }

        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath.Trim('"'));
        }

        public static string GetFileDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
    }
}
