using System.IO;

namespace github_release_poster
{
    public static class DebuggerDump
    {
        public static void Dump(this object element)
            => ObjectDumper.Write(element);

        public static void Dump(this object element, int depth)
            => ObjectDumper.Write(element, depth);

        public static void Dump(this object element, int depth, TextWriter log)
            => ObjectDumper.Write(element, depth, log);

        public static void DumpLines(this object element)
            => ObjectDumper.WriteLine(element);

        public static void DumpLines(this object element, int depth)
            => ObjectDumper.WriteLine(element, depth);

        public static void DumpLines(
            this object element,
            int depth,
            TextWriter log
        )
            => ObjectDumper.WriteLine(element, depth, log);
    }
}