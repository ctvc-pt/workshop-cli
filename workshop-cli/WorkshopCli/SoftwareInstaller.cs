using System;
using System.Diagnostics;
using System.IO;

namespace workshopCli
{
    public class SoftwareInstaller
    {
        private const string InstallPath = @"C:\Python39";
        private const string InstallerFileName = "python-installer.exe";
        private const string GitInstallerFileName = "Git-2.45.0-64-bit.exe";
        private const string VSCodeInstallerFileName = "VSCodeSetup"; // Name of the VS Code installer
        private static readonly string InstallerFilePath = Path.Combine(GuideCli.ResourcesPath, InstallerFileName);
        private static readonly string GitInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, GitInstallerFileName);
        private static readonly string VSCodeInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, VSCodeInstallerFileName);

        public void InstallPython()
        {
            if (IsPythonInstalled())
            {
                Console.WriteLine("O Python já esta instalado.");
            }
            else
            {
                Console.WriteLine("A instalar o Python...");
                RunInstaller(InstallerFilePath, $"/quiet InstallAllUsers=1 TargetDir=\"{InstallPath}\" PrependPath=1");
                Environment.SetEnvironmentVariable("PATH", $"{InstallPath};{Environment.GetEnvironmentVariable("PATH")}");
                Console.WriteLine("A instalação do Python foi un sucesso.");
            }

            // Install Git
            Console.WriteLine("A instalar o Git...");
            RunInstaller(GitInstallerFilePath, "/VERYSILENT /NORESTART");
            Console.WriteLine("A instalação do Git foi un sucesso.");

            // Install VS Code
            Console.WriteLine("A instalar o Visual Studio Code...");
            RunInstaller(VSCodeInstallerFilePath, "/verysilent /norestart");
            Console.WriteLine("A instalação do Visual Studio Code foi un sucesso.");

            // Install Lua Sumneko plugin
            Console.WriteLine("A instalar o Plugin Lua Sumneko para o VS Code...");
            InstallVSCodeExtension("sumneko.lua");
            Console.WriteLine("A instalação do Plugin Lua Sumneko foi un sucesso.");
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
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine($"Failed to install VS Code extension {extensionName}. Please check the logs for details.");
            }
        }
    }
}
