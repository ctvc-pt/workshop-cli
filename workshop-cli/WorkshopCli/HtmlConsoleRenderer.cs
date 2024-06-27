using System;
using System.Text.RegularExpressions;

namespace workshopCli
{
    public class HtmlConsoleRenderer
    {
        public static void Render(string text)
        {
            int position = 0;
            while (position < text.Length)
            {
                int startTagIndex = text.IndexOf('[', position);
                if (startTagIndex == -1)
                {
                    WriteWithoutWrapping(text.Substring(position));
                    break;
                }

                WriteWithoutWrapping(text.Substring(position, startTagIndex - position));

                int endTagIndex = text.IndexOf(']', startTagIndex);
                if (endTagIndex == -1)
                {
                    WriteWithoutWrapping(text.Substring(startTagIndex));
                    break;
                }

                string tagContent = text.Substring(startTagIndex + 1, endTagIndex - startTagIndex - 1);

                if (IsColorTag(tagContent))
                {
                    ProcessTag(tagContent);
                }
                else
                {
                    WriteWithoutWrapping(text.Substring(startTagIndex, endTagIndex - startTagIndex + 1));
                }

                position = endTagIndex + 1;
            }

            // Reset color at the end of rendering
            Console.ResetColor();
        }

        private static bool IsColorTag(string tagContent)
        {
            return Regex.IsMatch(tagContent, @"^(color=|/color)");
        }

        private static void WriteWithoutWrapping(string text)
        {
            Console.Write(text);
        }

        private static void ProcessTag(string tagContent)
        {
            if (tagContent.StartsWith("color=", StringComparison.OrdinalIgnoreCase))
            {
                var color = tagContent.Substring(6).Trim().ToLower();
                SetConsoleColor(color);
            }
            else if (tagContent.Equals("/color", StringComparison.OrdinalIgnoreCase))
            {
                Console.ResetColor();
            }
        }

        private static void SetConsoleColor(string color)
        {
            switch (color)
            {
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "black":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case "purple":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "orange":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }
}
