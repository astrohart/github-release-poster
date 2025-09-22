using System.Diagnostics;
using PostSharp.Patterns.Threading;
using github_release_poster;
using NUnit.Framework;
using System;
using System.IO;

namespace github_release_poster_tests
{
    [TestFixture, ExplicitlySynchronized]
    public class ZipperUpperTests
    {
        /// <summary> Fake, gibberish file/directory path </summary>
        private const string FakeFilePath =
            "@?%$asdlkfjaskldjhfsa;ldkjf;ksajdhds;kjagh";

        /// <summary>
        /// Valid path to a directory that commonly exists on Win10 systems --
        /// the Downloads folder! LOL
        /// </summary>
        public string ValidDirectoryPath { [DebuggerStepThrough] get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @"Dropbox\Downloads"
        );

        /// <summary>
        /// Valid path to a testing zip file to create.  Make this a file named
        /// after a GUID in a subfolder of the user's Desktop folder that is itself also
        /// named after a GUID.
        /// </summary>
        public string ValidZipFilePath { [DebuggerStepThrough] get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $@"{Guid.NewGuid()}\tempFiles_{Guid.NewGuid()}.zip"
        );

        /// <summary>
        /// Test to ensure the <see cref="T:github_release_poster.ZipperUpper" />
        /// class refuses to compress a folder when the folder name is valid but the
        /// destination ZIP file path is gibberish.
        /// </summary>
        [Test]
        public void CompressDirectoryTestDestFileNotExists()

            // Call the CompressDirectory method of the ZipperUpper method on purpose
            // passing a path to a directory whose name is valid but where the dest zip
            // file path is gibberish
            => Assert.IsFalse(
                ZipperUpper.CompressDirectory(ValidDirectoryPath, FakeFilePath)
            );

        /// <summary>
        /// Test to ensure the <see cref="T:github_release_poster.ZipperUpper" />
        /// class refuses to run when a blank or invalid directory has been passed.
        /// </summary>
        [Test]
        public void CompressDirectoryTestDirNotExists()

            // Call the CompressDirectory method of the ZipperUpper method on purpose
            // passing a path to a directory whose name is gibberish
            => Assert.IsFalse(
                ZipperUpper.CompressDirectory(FakeFilePath, ValidZipFilePath)
            );

        [Test]
        public void CompressDirectoryTestValidInputs()
        {
            Assert.IsTrue(
                ZipperUpper.CompressDirectory(
                    ValidDirectoryPath, ValidZipFilePath
                )
            );

            Assert.IsTrue(File.Exists(ValidZipFilePath));
        }
    }
}