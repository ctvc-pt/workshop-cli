using System;
using System.Diagnostics;
using System.IO;

namespace workshopCli
{
    public class PythonInstaller
    {
        private const string InstallPath = @"C:\Python39";
        private const string InstallerFileName = "python-installer.exe";
        private const string GitInstallerFileName = "Git-2.45.0-64-bit.exe";
        private const string VSCodeInstallerFileName = "VSCodeUserSetup-x64-1.89.1.exe"; // Name of the VS Code installer
        private static readonly string InstallerFilePath = Path.Combine(GuideCli.ResourcesPath, InstallerFileName);
        private static readonly string GitInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, GitInstallerFileName);
        private static readonly string VSCodeInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, VSCodeInstallerFileName);

        public void InstallPython()
        {
            if (IsPythonInstalled())
            {
                Console.WriteLine("Python is already installed.");
            }
            else
            {
                Console.WriteLine("Installing Python...");
                RunInstaller(InstallerFilePath, $"/quiet InstallAllUsers=1 TargetDir=\"{InstallPath}\" PrependPath=1");
                Environment.SetEnvironmentVariable("PATH", $"{InstallPath};{Environment.GetEnvironmentVariable("PATH")}");
                Console.WriteLine("Python installed successfully.");
            }

            // Install Git
            Console.WriteLine("Installing Git...");
            RunInstaller(GitInstallerFilePath, "/VERYSILENT /NORESTART");
            Console.WriteLine("Git installed successfully.");

            // Install VS Code
            Console.WriteLine("Installing Visual Studio Code...");
            RunInstaller(VSCodeInstallerFilePath, "/verysilent /norestart");
            Console.WriteLine("Visual Studio Code installed successfully.");

            // Install Lua Sumneko plugin
            Console.WriteLine("Installing Lua Sumneko plugin for VS Code...");
            InstallVSCodeExtension("sumneko.lua");
            Console.WriteLine("Lua Sumneko plugin installed successfully.");
        }

        private void RunInstaller(string filePath, string arguments)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            })?.WaitForExit();
        }

        private bool IsPythonInstalled()
        {
            var pythonProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            pythonProcess.Start();
            pythonProcess.WaitForExit();

            return pythonProcess.ExitCode == 0;
        }

        private void InstallVSCodeExtension(string extensionName)
        {
            var vscodeProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c code --install-extension {extensionName}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            vscodeProcess.Start();
            vscodeProcess.WaitForExit();

            if (vscodeProcess.ExitCode == 0)
            {
                Console.WriteLine($"VS Code extension {extensionName} installed successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to install VS Code extension {extensionName}. Please check the logs for details.");
            }
        }
    }
}
