using System.Diagnostics;
using System.IO.Compression;

namespace workshopCli;

public class GitHubManager
{
    public void CreateBranch()
    {
        //Console.WriteLine("1");
    
        var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_branch.py"; // Replace with the actual path to your Python script
    
        //Console.WriteLine("2");
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        //Console.WriteLine("3");
        try
        {
            var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error executing Python script: {ex.Message}");
        }
    }


    public void Commit(string name)
        {
            //Console.WriteLine("commit");
            var username = name;

            if (username != null)
            {
                username = username.Replace(" ", "-");
                //Console.WriteLine(username);
            }
            var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");

            var sourceFolderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}");

            var folderPath = Path.Combine(GuideCli.ResourcesPath, "Workshop2023");

            var files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var relativePath = file.Replace(sourceFolderPath, "").TrimStart(Path.DirectorySeparatorChar);
                var destPath = Path.Combine(folderPath, relativePath);
                var destDir = Path.GetDirectoryName(destPath);

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                    //Console.WriteLine($"Created folder: {destDir}");
                }

                File.Copy(file, destPath, true);
                //Console.WriteLine($"Copied file: {relativePath}");
            }

            var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_commit.py";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = pythonScriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                var process = new Process { StartInfo = processStartInfo };

                // Configure asynchronous event handlers for output and error data received
                //process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                //process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                // Start the process
                process.Start();

                // Begin asynchronous read operations for output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to exit
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing Python script: {ex.Message}");
            }
        }

   
}