using NUnit.Framework;
using ProjectReferencesBuilder.Helpers;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Tests.HelpersTests
{
    [Parallelizable(ParallelScope.All)]
    public class FileHelperTests
    {
        private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
        private static IEnumerable<TestCaseData> TestGetFileExtension_TestCases
        {
            get
            {
                yield return new TestCaseData(_mockedAbsolutePath).Returns(".csproj");
                yield return new TestCaseData(null).Returns(null);
            }
        }
        [TestCaseSource(nameof(TestGetFileExtension_TestCases))]
        public string TestGetFileExtension(string filePath)
        {
            return FileHelper.GetFileExtension(filePath);
        }

        private static IEnumerable<TestCaseData> TestGetFileDirectory_TestCases
        {
            get
            {
                yield return new TestCaseData(_mockedAbsolutePath);
                yield return new TestCaseData(null);
            }
        }
        [TestCaseSource(nameof(TestGetFileDirectory_TestCases))]
        public void TestGetFileDirectory(string filePath)
        {
            var fileDirectory = FileHelper.GetFileDirectory(filePath);

            if (filePath == null)
            {
                Assert.That(fileDirectory, Is.Null);
                return;
            }

            var fileDirectoryBackSlash = fileDirectory.Replace('/', '\\');
            var fileDirectoryForwardSlash = fileDirectory.Replace('\\', '/');

            Assert.That(fileDirectory, Is.EqualTo(fileDirectoryBackSlash).Or.EqualTo(fileDirectoryForwardSlash)); //annoying Windows vs Linux compatability issue. Need to check / and \
        }

        private static IEnumerable<TestCaseData> TestGetFileName_TestCases
        {
            get
            {
                yield return new TestCaseData(_mockedAbsolutePath).Returns("path");
                yield return new TestCaseData(null).Returns(null);
            }
        }
        [TestCaseSource(nameof(TestGetFileName_TestCases))]

        public string TestGetFileName(string filePath)
        {
            return FileHelper.GetFileName(filePath);
        }
    }
}
