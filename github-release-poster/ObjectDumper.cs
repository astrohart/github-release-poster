//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace github_release_poster
{
    public class ObjectDumper
    {
        private readonly int depth;

        private int level;

        private int pos;

        private TextWriter writer;

        private ObjectDumper(int depth)
        {
            this.depth = depth;
        }

        public static void Write(object element, int depth = 0)
        {
            Write(element, depth, Console.Out);
        }

        public static void Write(object element, int depth, TextWriter log)
        {
            var dumper = new ObjectDumper(depth) { writer = log };
            dumper.WriteObject(null, element);
        }

        public static void WriteLine(object element, int depth = 0)
        {
            WriteLine(element, depth, Console.Out);
        }

        public static void WriteLine(object element, int depth, TextWriter log)
        {
            var dumper = new ObjectDumper(depth) { writer = log };
            dumper.WriteObjectToLines(null, element);
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (var i = 0; i < level; i++) writer.Write("  ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (var item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level >= depth)
                            {
                                continue;
                            }
                            level++;
                            WriteObject(prefix, item);
                            level--;
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
                    var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteIndent();
                    Write(prefix);
                    var propWritten = false;
                    foreach (var m in members)
                    {
                        var f = m as FieldInfo;
                        var p = m as PropertyInfo;
                        if (f == null && p == null) continue;
                        if (propWritten)
                        {
                            WriteTab();
                        }
                        else
                        {
                            propWritten = true;
                        }
                        Write(m.Name);
                        Write("=");
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || t == typeof(string))
                        {
                            WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                        }
                        else
                        {
                            if (typeof(IEnumerable).IsAssignableFrom(t))
                            {
                                Write("...");
                            }
                            else
                            {
                                Write("{ }");
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (var m in members)
                        {
                            var f = m as FieldInfo;
                            var p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                var t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        level++;
                                        WriteObject(m.Name + ": ", value);
                                        level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteObjectToLines(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteLine();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (var item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteLine();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level >= depth)
                            {
                                continue;
                            }
                            level++;
                            WriteObjectToLines(prefix, item);
                            level--;
                        }
                        else
                        {
                            WriteObjectToLines(prefix, item);
                        }
                    }
                }
                else
                {
                    var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteLine();
                    Write(prefix);
                    var propWritten = false;
                    foreach (var m in members)
                    {
                        var f = m as FieldInfo;
                        var p = m as PropertyInfo;
                        if (f == null && p == null) continue;
                        if (propWritten)
                        {
                            WriteLine();
                        }
                        else
                        {
                            propWritten = true;
                        }
                        Write(m.Name);
                        Write("=");
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || t == typeof(string))
                        {
                            WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                        }
                        else
                        {
                            Write(typeof(IEnumerable).IsAssignableFrom(t) ? "..." : "{ }");
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (var m in members)
                        {
                            var f = m as FieldInfo;
                            var p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                var t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        level++;
                                        WriteObjectToLines(m.Name + ": ", value);
                                        level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteTab()
        {
            Write("  ");
            while (pos % 8 != 0) Write(" ");
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}