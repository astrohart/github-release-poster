using System.Linq;
using github_release_poster;
using NUnit.Framework;

namespace github_release_poster_tests
{
    [TestFixture]
    public class CommandLineInfoTests
    {
        [Test]
        public void ParseCommandLineTesWithNoArgs()
        {
            Assert.IsFalse(
                CommandLineInfo.Instance.ParseCommandLine(Enumerable.Empty<string>().ToArray())
            );

            Assert.IsFalse(CommandLineInfo.Instance.WasCommandLineParsed);
        }
    }
}