using System;
using System.Text.RegularExpressions;

namespace workshopCli
{
    public class HtmlConsoleRenderer
    {
        public static void Render(string text)
        {
            lock (RenderState.RenderLock)
            {
                RenderState.CurrentMarkdown = text;
                RenderInternal(text);
            }
        }

        public static void RenderInternal(string text)
        {
            ConsoleColor? currentColor = null;
            int position = 0;
            int width = GetWrapWidth();

            while (position < text.Length)
            {
                int tagStart = text.IndexOf('[', position);
                if (tagStart == -1)
                {
                    EmitSegment(text.Substring(position), currentColor, width);
                    break;
                }

                if (tagStart > position)
                {
                    EmitSegment(text.Substring(position, tagStart - position), currentColor, width);
                }

                int tagEnd = text.IndexOf(']', tagStart);
                if (tagEnd == -1)
                {
                    EmitSegment(text.Substring(tagStart), currentColor, width);
                    break;
                }

                string tagContent = text.Substring(tagStart + 1, tagEnd - tagStart - 1);
                if (IsColorTag(tagContent))
                {
                    currentColor = ProcessTag(tagContent);
                }
                else
                {
                    EmitSegment(text.Substring(tagStart, tagEnd - tagStart + 1), currentColor, width);
                }

                position = tagEnd + 1;
            }

            Console.ResetColor();
        }

        private static void EmitSegment(string content, ConsoleColor? color, int width)
        {
            if (content.Length == 0) return;
            ApplyColor(color);
            if (IsWrappable(color))
            {
                WriteWrapped(content, width);
            }
            else
            {
                Console.Write(content);
            }
        }

        private static bool IsWrappable(ConsoleColor? color)
        {
            // Default (no tag) and explicit white are prose; everything else
            // (blue=code, red/green=ASCII-art) must be passed verbatim or
            // we destroy the layout.
            return color is null || color == ConsoleColor.White;
        }

        private static void WriteWrapped(string content, int width)
        {
            // Paragraphs are separated by a blank line. Within a paragraph we
            // collapse the hand-formatted line breaks (the .md files are
            // pre-wrapped to ~70 chars) and re-wrap to the actual terminal
            // width, so words don't get cut in half on narrow terminals.
            var paragraphs = Regex.Split(content, @"\r?\n[ \t]*\r?\n");
            bool first = true;
            foreach (var raw in paragraphs)
            {
                string paragraph = Regex.Replace(raw, @"\s+", " ").Trim();
                if (paragraph.Length == 0)
                {
                    if (!first) Console.WriteLine();
                    continue;
                }
                if (!first) Console.WriteLine();
                WriteWordWrapped(paragraph, width);
                Console.WriteLine();
                first = false;
            }
        }

        private static void WriteWordWrapped(string paragraph, int width)
        {
            var words = paragraph.Split(' ');
            int lineLength = 0;
            foreach (var word in words)
            {
                if (word.Length == 0) continue;
                if (lineLength == 0)
                {
                    Console.Write(word);
                    lineLength = word.Length;
                }
                else if (lineLength + 1 + word.Length <= width)
                {
                    Console.Write(' ');
                    Console.Write(word);
                    lineLength += 1 + word.Length;
                }
                else
                {
                    Console.WriteLine();
                    Console.Write(word);
                    lineLength = word.Length;
                }
            }
        }

        private static int GetWrapWidth()
        {
            try
            {
                int w = Console.WindowWidth;
                return w > 2 ? w - 2 : 80;
            }
            catch
            {
                return 80;
            }
        }

        private static bool IsColorTag(string tagContent)
        {
            return Regex.IsMatch(tagContent, @"^(color=|/color)");
        }

        private static ConsoleColor? ProcessTag(string tagContent)
        {
            if (tagContent.StartsWith("color=", StringComparison.OrdinalIgnoreCase))
            {
                return ParseColor(tagContent.Substring(6).Trim().ToLower());
            }
            return null;
        }

        private static ConsoleColor? ParseColor(string color)
        {
            return color switch
            {
                "white" => ConsoleColor.White,
                "black" => ConsoleColor.Black,
                "purple" => ConsoleColor.Magenta,
                "green" => ConsoleColor.Green,
                "yellow" => ConsoleColor.Yellow,
                "red" => ConsoleColor.Red,
                "blue" => ConsoleColor.Blue,
                "cyan" => ConsoleColor.Cyan,
                "darkcyan" => ConsoleColor.DarkCyan,
                "orange" => ConsoleColor.DarkYellow,
                _ => null,
            };
        }

        private static void ApplyColor(ConsoleColor? color)
        {
            if (color is null)
            {
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = color.Value;
            }
        }
    }
}
