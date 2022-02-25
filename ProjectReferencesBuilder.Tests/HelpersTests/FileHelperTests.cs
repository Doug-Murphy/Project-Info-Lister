using FluentAssertions;
using ProjectReferencesBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ProjectReferencesBuilder.Tests.HelpersTests;

public class FileHelperTests
{
    private const string _mockedAbsolutePath = @"dummy/absolute/path.csproj";
    public static IEnumerable<object[]> TestGetFileExtension_TestCases()
    {
        yield return new object[] { ".csproj", _mockedAbsolutePath };
        yield return new object[] { null, null };
    }
    [Theory]
    [MemberData(nameof(TestGetFileExtension_TestCases))]
    public void TestGetFileExtension(string expectedResult, string filePath)
    {
        Assert.Equal(expectedResult, FileHelper.GetFileExtension(filePath));
    }

    public static IEnumerable<object[]> TestGetFileDirectory_TestCases()
    {
        yield return new object[] { _mockedAbsolutePath };
        yield return new object[] { null };
    }
    [Theory]
    [MemberData(nameof(TestGetFileDirectory_TestCases))]
    public void TestGetFileDirectory(string filePath)
    {
        var fileDirectory = FileHelper.GetFileDirectory(filePath);

        if (filePath == null)
        {
            Assert.Null(fileDirectory);
            return;
        }

        var fileDirectoryBackSlash = fileDirectory.Replace('/', '\\');
        var fileDirectoryForwardSlash = fileDirectory.Replace('\\', '/');

        fileDirectory.Should().BeOneOf(fileDirectoryBackSlash, fileDirectoryForwardSlash); //annoying Windows vs Linux compatability issue. Need to check / and \
    }

    public static IEnumerable<object[]> TestGetFileName_TestCases()
    {
        yield return new object[] { "path", _mockedAbsolutePath };
        yield return new object[] { null, null };
    }
    [Theory]
    [MemberData(nameof(TestGetFileName_TestCases))]
    public void TestGetFileName(string expectedResult, string filePath)
    {
        Assert.Equal(expectedResult, FileHelper.GetFileName(filePath));
    }

    [Fact]
    public void TestEmptyFileThrowsException()
    {
        var emptyFilePath = Path.GetTempFileName();
        Assert.Throws<InvalidOperationException>(() => { FileHelper.GetFileContents(emptyFilePath); });
    }
}