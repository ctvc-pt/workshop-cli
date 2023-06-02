using System.Diagnostics;

namespace workshopCli;

public class PythonInstaller
{
    private const string PythonInstallerUrl = "https://www.python.org/ftp/python/3.9.5/python-3.9.5-amd64.exe";
    private const string InstallPath = @"C:\Python39";

    public void InstallPython()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "curl",
            Arguments = $"-o python-installer.exe {PythonInstallerUrl}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })
            ?.WaitForExit();

        Process.Start(new ProcessStartInfo
        {
            FileName = "python-installer.exe",
            Arguments = $"/quiet InstallAllUsers=1 TargetDir=\"{InstallPath}\" PrependPath=1",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })
            ?.WaitForExit();

        Environment.SetEnvironmentVariable("PATH", $"{InstallPath};{Environment.GetEnvironmentVariable("PATH")}");

        Process.Start(new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "--version",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })
            ?.WaitForExit();

        Process.Start(new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "-m pip install PyGithub",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })
            ?.WaitForExit();
    }


}