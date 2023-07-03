using System;
using System.Diagnostics;

namespace workshopCli
{
    public class ProcessLuv
    {
        public void CloseLovecProcess()
        {
            Process[] processes = Process.GetProcessesByName("lovec");

            foreach (Process process in processes)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }

            //Console.WriteLine("lovec.exe process closed.");
        }
    }
}