using System;
using System.Diagnostics;

namespace workshopCli
{
    public class ProcessLuv
    {
        public void CloseLovecProcess()
        {
            foreach (var processName in new[] { "lovec", "love" })
            {
                foreach (Process process in Process.GetProcessesByName(processName))
                {
                    process.CloseMainWindow();
                    if (!process.WaitForExit(2000))
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
            }
        }
    }
}
