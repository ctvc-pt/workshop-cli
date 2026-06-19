using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class CreateSpritesAction : IAction
{
    public int Delay;
    private int guideNumber;

    public CreateSpritesAction(int delay, bool isGuide2)
    {
        Delay = delay;
        this.guideNumber = isGuide2 ? 2 : 1;
    }

    public CreateSpritesAction(int delay, int guideNumber)
    {
        Delay = delay;
        this.guideNumber = guideNumber;
    }

    public void Execute()
    {
        // Escolhe a pasta de origem com base no guia ativo
        var imageFolder = guideNumber switch
        {
            2 => "imagesGuide2",
            3 => "imagesGuide3",
            _ => "Images"
        };
        var sourceFolderPath = Path.Combine(GuideCli.ResourcesPath, imageFolder);
        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
        var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");

        if (!File.Exists(txtFilePath))
        {
            return;
        }

        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
        var username = session.Name;
        if (username != null) username = username.Replace(" ", "-");

        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Verifica se a pasta de origem existe
        if (!Directory.Exists(sourceFolderPath))
        {
            Console.WriteLine($"Pasta de imagens não encontrada: {sourceFolderPath}");
            return;
        }

        string[] files = Directory.GetFiles(sourceFolderPath);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(folderPath, fileName);
            File.Copy(file, destPath, true);
        }

        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' ou 'a'. Se por alguma razão desejares voltar ao codigo anterior, escreve 'reset'");
    }
}
