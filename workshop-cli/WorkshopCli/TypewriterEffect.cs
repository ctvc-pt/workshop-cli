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

            foreach (string word in words)
            {
                Console.Write(word + " ");
                Thread.Sleep(delay);
            }

            Console.ResetColor();
        }
    }
}