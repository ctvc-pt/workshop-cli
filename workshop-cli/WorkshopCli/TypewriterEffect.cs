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

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }

            Console.ResetColor();
        }
    }
}