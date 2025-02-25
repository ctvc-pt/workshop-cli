
using System;
using System.Runtime.InteropServices;
using System.Threading;
namespace workshopCli;
public class KeyPress
{
    private const int VK_DOWN = 0x28;
    private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const int KEYEVENTF_KEYUP = 0x0002;

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    public static void SimulateKeyPress()
    {
        // Press the Down key
        keybd_event(VK_DOWN, 0, KEYEVENTF_EXTENDEDKEY, 0);

        // Wait for a short duration
        Thread.Sleep(100);

        // Release the Down key
        keybd_event(VK_DOWN, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }
}

