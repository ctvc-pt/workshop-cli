using System.Diagnostics;

namespace workshopCli;

public class PythonInstaller
{
    private const string InstallPath = @"C:\Python39";
    private const string InstallerFileName = "python-installer.exe";
    private static readonly string InstallerFilePath = Path.Combine(GuideCli.ResourcesPath, InstallerFileName);

    public void InstallPython()
    {
        //Console.WriteLine("Installing Python...");
        Process.Start(new ProcessStartInfo
        {
            FileName = InstallerFilePath,
            Arguments = $"/quiet InstallAllUsers=1 TargetDir=\"{InstallPath}\" PrependPath=1",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })?.WaitForExit();

        //Console.WriteLine("Updating PATH environment variable...");
        Environment.SetEnvironmentVariable("PATH", $"{InstallPath};{Environment.GetEnvironmentVariable("PATH")}");

       //Console.WriteLine("Verifying Python installation...");
        Process.Start(new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "--version",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })?.WaitForExit();

        //Console.WriteLine("Installing PyGithub package...");
        Process.Start(new ProcessStartInfo
        {
            FileName = "python",
            Arguments = "-m pip install PyGithub",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        })?.WaitForExit();
    }
}