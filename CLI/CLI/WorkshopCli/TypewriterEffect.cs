using System;
using System.Threading;

namespace workshopCli
{
    public class TypewriterEffect
    {
        private int delay;

        public TypewriterEffect(int delay)
        {
            this.delay = delay;
        }

        public void Type(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;

            string[] words = text.Split(' ');
            int consoleWidth = Console.WindowWidth;
            int currentLineLength = 0;

            foreach (string word in words)
            {
                // Check if the word will exceed the console width
                if (currentLineLength + word.Length + 1 > consoleWidth)
                {
                    // Move to the next line if thats the case
                    Console.WriteLine();
                    currentLineLength = 0;
                }

                // Print the word that was cut
                Console.Write(word + " ");
                currentLineLength += word.Length + 1; 
                Thread.Sleep(delay);
            }

            Console.ResetColor();
        }
    }
}