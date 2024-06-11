using System;
using System.Text.RegularExpressions;

namespace workshopCli
{
    public class HtmlConsoleRenderer
    {
        public static void Render(string html)
        {
            // Find HTML tags and apply styles or colors to text in the console
            int position = 0;
            while (position < html.Length)
            {
                // Find the next opening tag
                int startTagIndex = html.IndexOf('<', position);
                if (startTagIndex == -1)
                {
                    WriteLineWithoutWrapping(html.Substring(position));
                    break;
                }

                WriteLineWithoutWrapping(html.Substring(position, startTagIndex - position));

                // Find the next closing tag
                int endTagIndex = html.IndexOf('>', startTagIndex);
                if (endTagIndex == -1)
                {
                    WriteLineWithoutWrapping(html.Substring(position));
                    break;
                }

                // Extract the tag
                string tag = html.Substring(startTagIndex, endTagIndex - startTagIndex + 1);
                ProcessTag(tag);

                // Update the position to the end of the tag
                position = endTagIndex + 1;
            }
        }

        private static void WriteLineWithoutWrapping(string text)
        {
            string[] lines = text.Split('\n');

            // Verifica se o texto é vazio ou contém apenas espaços em branco
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine(); // Se for vazio, apenas imprime uma quebra de linha
                return;
            }

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim(); // Remove espaços em branco extras no início e no final da linha
                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    Console.WriteLine(); // Se a linha contém apenas espaços em branco, imprime uma quebra de linha
                }
                else
                {
                    PrintWithoutWrapping(trimmedLine); // Imprime a linha sem cortar as palavras
                }
            }
        }

        private static void PrintWithoutWrapping(string line)
        {
            string[] words = line.Split(' '); // Divide a linha em palavras
            int consoleWidth = Console.WindowWidth;
            int currentLineLength = 0;

            foreach (string word in words)
            {
                if (currentLineLength + word.Length + 1 > consoleWidth) // Verifica se a palavra cabe na linha atual
                {
                    Console.WriteLine(); // Se não couber, imprime uma quebra de linha
                    Console.Write(word + " ");
                    currentLineLength = word.Length + 1;
                }
                else
                {
                    Console.Write(word + " "); // Se couber, imprime a palavra na mesma linha
                    currentLineLength += word.Length + 1;
                }
            }
            Console.WriteLine(); // Imprime uma quebra de linha no final da linha
        }



        
        private static void ProcessTag(string tag)
        {
            // Check if the tag is a <span> or <pre> with defined style
            if (tag.StartsWith("<span") && tag.Contains("style=") || tag.StartsWith("<pre") && tag.Contains("style="))
            {
                // Extract the style from the tag
                var match = Regex.Match(tag, @"style\s*=\s*""([^""]*)""");
                if (match.Success)
                {
                    var style = match.Groups[1].Value;
                    var colorMatch = Regex.Match(style, @"color\s*:\s*([^;]*)");
                    if (colorMatch.Success)
                    {
                        var color = colorMatch.Groups[1].Value;
                        // Apply corresponding color to the console
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
                // If the tag is not a <span> or <pre> with style, reset console colors
                Console.ResetColor();
            }
        }
    }
}
