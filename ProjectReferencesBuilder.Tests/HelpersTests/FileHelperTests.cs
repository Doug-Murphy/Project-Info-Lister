using NUnit.Framework;
using ProjectReferencesBuilder.Helpers;
using System.Collections.Generic;

namespace ProjectReferencesBuilder.Tests.HelpersTests
{
    [Parallelizable(ParallelScope.All)]
    public class FileHelperTests
    {
        private static IEnumerable<TestCaseData> TestGetFileExtension_TestCases
        {
            get
            {
                yield return new TestCaseData(@"C:\ProjectInfoTester\File.sln").Returns(".sln");
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
                yield return new TestCaseData(@"C:\ProjectInfoTester\File.sln").Returns(@"C:\ProjectInfoTester");
                yield return new TestCaseData(null).Returns(null);
            }
        }
        [TestCaseSource(nameof(TestGetFileDirectory_TestCases))]
        public string TestGetFileDirectory(string filePath)
        {
            return FileHelper.GetFileDirectory(filePath);
        }

        private static IEnumerable<TestCaseData> TestGetFileName_TestCases
        {
            get
            {
                yield return new TestCaseData(@"C:\ProjectInfoTester\File.sln").Returns("File");
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
