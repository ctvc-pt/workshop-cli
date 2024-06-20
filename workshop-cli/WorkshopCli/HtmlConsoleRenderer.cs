using System;
using System.Text.RegularExpressions;

namespace workshopCli
{
    public class HtmlConsoleRenderer
    {
        public static void Render(string html)
        {
            int position = 0;
            while (position < html.Length)
            {
                int startTagIndex = html.IndexOf('<', position);
                if (startTagIndex == -1)
                {
                    WriteLineWithoutWrapping(html.Substring(position));
                    break;
                }

                WriteLineWithoutWrapping(html.Substring(position, startTagIndex - position));

                int endTagIndex = html.IndexOf('>', startTagIndex);
                if (endTagIndex == -1)
                {
                    WriteLineWithoutWrapping(html.Substring(startTagIndex));
                    break;
                }

                string tagContent = html.Substring(startTagIndex + 1, endTagIndex - startTagIndex - 1);

                if (IsTag(tagContent))
                {
                    string tag = html.Substring(startTagIndex, endTagIndex - startTagIndex + 1);
                    ProcessTag(tag);
                }
                else
                {
                    WriteLineWithoutWrapping(html.Substring(startTagIndex, endTagIndex - startTagIndex + 1));
                }

                position = endTagIndex + 1;
            }
        }

        private static bool IsTag(string tagContent)
        {
            return Regex.IsMatch(tagContent, @"^\s*/?\s*\w+.*$");
        }

        private static void WriteLineWithoutWrapping(string text)
        {
            string[] lines = text.Split('\n');

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine();
                return;
            }

            foreach (string line in lines)
            {
                PrintWithoutWrapping(line);
            }
        }

        private static void PrintWithoutWrapping(string line)
        {
            string[] words = line.Split(' ');
            int consoleWidth = Console.WindowWidth;
            int currentLineLength = 0;

            foreach (string word in words)
            {
                if (currentLineLength + word.Length + 1 > consoleWidth)
                {
                    Console.WriteLine();
                    Console.Write(word + " ");
                    currentLineLength = word.Length + 1;
                }
                else
                {
                    Console.Write(word + " ");
                    currentLineLength += word.Length + 1;
                }
            }
            Console.WriteLine();
        }

        private static void ProcessTag(string tag)
        {
            if (tag.StartsWith("<span") && tag.Contains("style=") || tag.StartsWith("<pre") && tag.Contains("style="))
            {
                var match = Regex.Match(tag, @"style\s*=\s*""([^""]*)""");
                if (match.Success)
                {
                    var style = match.Groups[1].Value;
                    var colorMatch = Regex.Match(style, @"color\s*:\s*([^;]*)");
                    if (colorMatch.Success)
                    {
                        var color = colorMatch.Groups[1].Value;
                        switch (color.ToLower())
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
                        }
                    }
                }
            }
            else
            {
                Console.ResetColor();
            }
        }
    }
}
