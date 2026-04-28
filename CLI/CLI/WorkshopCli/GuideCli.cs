using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace workshopCli
{
    public class GuideCli
    {
        public static string ResourcesPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..", "..", "..", "Resources");
        public Session session;
        public Guide guide;
        CsvController csvController = new CsvController();
        public static int adminInput;
        public static string stepMessage;

        public GuideCli(Guide guide)
        {
            this.guide = guide;
            session = new Session();
        }

        public void Run()
{
    var VsCode = new OpenVSCode();
    var assembly = Assembly.GetExecutingAssembly();
    var txtFilePath = Path.Combine(ResourcesPath, "session.txt");

    // Carregar sessão existente, se houver
    if (File.Exists(txtFilePath))
    {
        try
        {
            string jsonContent = File.ReadAllText(txtFilePath);
            session = JsonConvert.DeserializeObject<Session>(jsonContent) ?? new Session();
            if (!string.IsNullOrEmpty(session.Name))
            {
                Console.WriteLine($"Bem-vindo outra vez {session.Name}!");
            }
            else
            {
                Console.WriteLine("Sessão carregada, mas nome não encontrado. Iniciando como novo usuário.");
                session = new Session();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar a sessão: {ex.Message}. Criando nova sessão.");
            session = new Session(); // Fallback para uma nova sessão
        }
    }
    else
    {
        Console.WriteLine("Bem-vindo ao Workshop de Love2D!");
        session = new Session(); // Inicializa se não houver arquivo
    }

    // Executa a ação de participação (pulará se StepId existir)
    var askParticipation = new AskParticipationAction(this);
    askParticipation.Execute();

    // Determinar o guia com base na resposta (ou manter o salvo)
    var participation = session.Participation?.Trim().ToLower();

    // Se disse "sim", pergunta qual guia quer seguir (Flappy Bird ou Endless Skater)
    if (participation == "sim" && string.IsNullOrEmpty(session.StepId))
    {
        int cursorTop = Console.CursorTop;
        Console.WriteLine("Qual jogo queres fazer?");
        Console.WriteLine("  1 - Flappy Bird");
        Console.WriteLine("  2 - Endless Skater\n");

        string escolha;
        do
        {
            escolha = ExerciseHelper.PromptAnswerAndPrint()?.Trim();
            if (escolha != "1" && escolha != "2")
            {
                Console.WriteLine("Resposta invalida. Por favor, responde '1' ou '2'.");
            }
        } while (escolha != "1" && escolha != "2");

        participation = escolha == "1" ? "2" : "3";
        session.Participation = participation;
    }

    int guideNumber = participation switch
    {
        "2" or "sim" => 2,
        "3" => 3,
        _ => 1
    };
    bool isGuide2 = guideNumber >= 2;
    guide.SetSteps(guideNumber);

    // Iniciar o loop a partir do StepId salvo ou do início
    var startIndex = guide.Steps.FindIndex(step => step.Id == session.StepId);
    if (startIndex == -1)
    {
        startIndex = 0;
    }
    else
    {
        startIndex += 1; // Avança para o próximo passo após o salvo
    }

    int currentIndex = 0;
    for (var i = startIndex; i < guide.Steps.Count; i++)
    {
        var step = guide.Steps[i];
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(step.Id);
        Console.ForegroundColor = ConsoleColor.Black;
        session.StepId = step.Id;
        currentIndex = i;

        if (step.Type == "information" || step.Type == "challenge" || step.Type == "CreateSprites")
        {
            VsCode.Open();
        }
        if (step.Type != "code" && step.Type != "open-file")
        {
            var filePath = $"{step.Id}.md";
            string resourcePrefix = guideNumber switch
            {
                2 => "workshop_cli.Guide_2.",
                3 => "workshop_cli.Guide_3.",
                _ => "workshop_cli.Guide."
            };
            var resourceName = $"{resourcePrefix}{filePath}";
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream != null)
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    var markdown = reader.ReadToEnd();
                    WrapString(markdown, 100);
                    HtmlConsoleRenderer.Render(markdown);
                }
            }
            else
            {
                Console.WriteLine($"Resource '{resourceName}' not found for step {step.Id}");
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        if (i > 4)
            Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(step.Message);
        Console.ForegroundColor = ConsoleColor.White;

        stepMessage = step.Message;

        var delay = step.Delay;

        var actions = new Dictionary<string, IAction>()
        {
            { "information", new InformationAction(this, delay) },
            { "challenge", new ChallengeAction(delay, session.Name) },
            { "install", new InstallAction() },
            { "open-file", new OpenFileL2DAction(step.Id, delay) },
            { "CreateSprites", new CreateSpritesAction(delay, guideNumber) },
            { "code", new CodeAction(currentIndex, guideNumber) },
            { "ask-name", new AskNameAction(this) },
            { "ask-age", new AskAgeAction(this) },
            { "ask-email", new AskEmailAction(this) },
            { "ask-mesa", new AskTableAction(this) },
            { "video", new VideoAction(currentIndex, this) },
            { "end", new EndAction() }
        };

        if (actions.TryGetValue(step.Type, out var action))
        {
            action.Execute();
            if (adminInput == -1)
            {
                i = i - 2;
            }
            else if (adminInput != 0)
            {
                i = adminInput - 1;
            }
            adminInput = 0;
        }
        else
        {
            Console.WriteLine($"Unknown action type: {step.Type}");
        }

        var NameId = session.Name + session.Mesa;
        session.NameId = NameId;
        if (i >= 4)
        {
            bool isChallenge = step.Type == "challenge";
            var snapshot = (Name: session.Name, Age: session.Age, Email: session.Email, StepId: session.StepId, NameId: NameId, IsChallenge: isChallenge);
            _ = System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    csvController.UpdateSession(snapshot.Name, snapshot.Age, snapshot.Email, snapshot.StepId, snapshot.NameId, snapshot.IsChallenge);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Os dados não foram guardados: {e.Message}");
                }
            });
        }

        File.WriteAllText(txtFilePath, JsonConvert.SerializeObject(session));

        Console.Clear();
    }

    Thread.Sleep(2000);
    Environment.Exit(0);
}
        public static string WrapString(string input, int width)
        {
            string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            StringBuilder wrappedString = new StringBuilder();

            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                StringBuilder currentLine = new StringBuilder();
                int currentLineLength = 0;

                foreach (string word in words)
                {
                    if (currentLineLength + word.Length + 1 > width)
                    {
                        wrappedString.AppendLine(currentLine.ToString().TrimEnd());
                        currentLine.Clear();
                        currentLineLength = 0;
                    }

                    currentLine.Append(word + " ");
                    currentLineLength += word.Length + 1;
                }

                wrappedString.AppendLine(currentLine.ToString().TrimEnd());
            }

            return wrappedString.ToString().TrimEnd();
        }
        
        
    }
}


