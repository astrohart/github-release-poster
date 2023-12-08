using PostSharp.Patterns.Threading;
using NUnit.Framework;

namespace github_release_poster_tests
{
    [TextFixture, ExplicitlySynchronized]
    public class ProgramTests { }
}