using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace workshopCli
{
    public class Timers
    {
        private Timer timer;
        
        
        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        public void StartTimer()
        {
            // Create and start the timer
            timer = new Timer(TimerCallback, null, 20000, Timeout.Infinite);
        }
        public void CancelTimer()
        {
            // Stop and dispose of the timer
            timer?.Dispose();
        }
        private void TimerCallback(object state)
        {
            // Code to execute when the timer elapses
            //Console.WriteLine("Hello from the timer!");

            // Get the CLI window handle
            IntPtr consoleWindowHandle = GetConsoleWindow();

            // Set focus to the CLI window
            SetForegroundWindow(consoleWindowHandle);

            // Simulate key press
            KeyPress.SimulateKeyPress();
        }
    }
}