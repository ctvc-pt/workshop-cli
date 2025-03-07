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
        private const string VSCodeInstallerFileName = "VSCodeSetup.exe"; // Corrected file name with extension
        private static readonly string InstallerFilePath = Path.Combine(GuideCli.ResourcesPath, InstallerFileName);
        private static readonly string GitInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, GitInstallerFileName);
        private static readonly string VSCodeInstallerFilePath = Path.Combine(GuideCli.ResourcesPath, VSCodeInstallerFileName);
        private static bool isInstalled = false;

        public void InstallSoftware()
        {
            if (isInstalled)
            {
                Console.WriteLine("Software already installed.");
                return;
            }

            InstallPython();
            InstallGit();
            InstallVSCode();
            InstallVSCodeExtension("sumneko.lua");

            isInstalled = true;
        }

        public void InstallPython()
        {
            if (IsPythonInstalled())
            {
                Console.WriteLine("O Python já está instalado.");
            }
            else
            {
                Console.WriteLine("A instalar o Python...");
                RunInstaller(InstallerFilePath, $"/quiet InstallAllUsers=1 TargetDir=\"{InstallPath}\" PrependPath=1");
                Environment.SetEnvironmentVariable("PATH", $"{InstallPath};{Environment.GetEnvironmentVariable("PATH")}");
                Console.WriteLine("A instalação do Python foi um sucesso.");
            }
        }

        public void InstallGit()
        {
            Console.WriteLine("A instalar o Git...");
            RunInstaller(GitInstallerFilePath, "/VERYSILENT");
            Console.WriteLine("A instalação do Git foi um sucesso.");
        }

        public void InstallVSCode()
        {
            if ( IsVSCodeInstalled() )
            {
                Console.WriteLine("O VS Code já está instalado.");
            }
            else
            {
                Console.WriteLine("A instalar o Visual Studio Code...");
                RunInstaller(VSCodeInstallerFilePath, "/verysilent /norestart");
                Console.WriteLine("A instalação do Visual Studio Code foi um sucesso.");
                foreach (var process in Process.GetProcessesByName("Code")) 
                { 
                    process.Kill();
                }
            }
            
        }

        public void InstallVSCodeExtension(string extensionName)
        {
            if(IsVSCodeInstalled()){
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

                if ( vscodeProcess.ExitCode == 0 )
                {
                    Console.WriteLine( $"VS Code extension {extensionName} installed successfully." );
                }
                else
                {
                    Console.WriteLine(
                        $"Failed to install VS Code extension {extensionName}. Please check the logs for details." );
                }
            }
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

        private bool IsVSCodeInstalled()
        {
            string path = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "..", "Local/Programs/Microsoft VS Code", "Code.exe" );
            return File.Exists(path);
        }
    }
}
