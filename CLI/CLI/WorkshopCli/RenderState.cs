using System;

namespace workshopCli
{
    public static class RenderState
    {
        public static readonly object RenderLock = new object();

        public static string CurrentMarkdown = string.Empty;
        public static string CurrentTrailingMessage = string.Empty;
        public static ConsoleColor CurrentTrailingColor = ConsoleColor.White;
        public static bool IsPrompting = false;

        public static void Clear()
        {
            lock (RenderLock)
            {
                CurrentMarkdown = string.Empty;
                CurrentTrailingMessage = string.Empty;
                IsPrompting = false;
            }
        }
    }
}
