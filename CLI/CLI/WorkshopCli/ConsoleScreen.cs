using System;

namespace workshopCli
{
    public static class ConsoleScreen
    {
        public static void Clear()
        {
            try
            {
                Console.Write("\x1b[2J\x1b[3J\x1b[H");
            }
            catch
            {
                // Older consoles may not support ANSI. Fall back below.
            }

            try
            {
                Console.Clear();
            }
            catch
            {
                // Fall back to manually blanking the visible window below.
            }

            try
            {
                var width = Math.Max(1, Console.WindowWidth - 1);
                var height = Math.Max(1, Console.WindowHeight);
                var blank = new string(' ', width);

                for (var row = 0; row < height; row++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(blank);
                }

                Console.SetCursorPosition(0, 0);
            }
            catch
            {
                // Some terminals throw while being resized. Best effort is enough here.
            }
        }
    }
}
