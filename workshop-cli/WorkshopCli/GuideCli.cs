using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Markdig;

namespace workshopCli
{
    public class GuideCli
    {
        public static string ResourcesPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources");
        public Session session;
        public Guide guide;
        CsvSessionWriter sessionWriter = new CsvSessionWriter();
        CsvHelpRequest helpRequest = new CsvHelpRequest();
        public static int adminInput;

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

            if (File.Exists(txtFilePath))
            {
                session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
                Console.WriteLine($"Bem-vindo outra vez {session.Name}!");
            }
            else
            {
                Console.WriteLine("Bem-vindo ao Workshop de Love2D!");
            }

            var startIndex = guide.Steps.FindIndex(step => step.Id == session.StepId);
            if (startIndex == -1)
            {
                startIndex = 0;
            }
            else
            {
                startIndex += 1;
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
                if (step.Type != "code" && step.Type != "open-file" && step.Type != "intro")
                {
                    var filePath = $"{step.Id}.md";
                    var resourceStream = assembly.GetManifestResourceStream($"workshop_cli.Guide.{filePath}");
                    if (resourceStream != null)
                    {
                        using (var reader = new StreamReader(resourceStream))
                        {
                            var markdown = reader.ReadToEnd();
                            // Convert Markdown to HTML without modifying special characters
                            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                            var html = Markdown.ToHtml(markdown, pipeline);
                            // Replace &quot; with "
                            html = html.Replace("&quot;", "\"");
                            html = html.Replace("&gt;", ">");
                            html = html.Replace("&lt;", "<");
                            html = html.Replace("</p>", "");
                            html = html.Replace("</span>", "");
                            
                            HtmlConsoleRenderer.Render(html);
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                if (i > 4)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(step.Message);
                Console.ForegroundColor = ConsoleColor.White;


                var delay = step.Delay;
                delay = 1;

                var actions = new Dictionary<string, IAction>()
                {
                    { "information", new InformationAction(this, delay) },
                    { "challenge", new ChallengeAction(delay, session.Name) },
                    { "intro", new CreateBranchAction() },
                    { "install", new InstallAction() },
                    { "open-file", new OpenFileL2DAction(step.Id, delay) },
                    { "CreateSprites", new CreateSpritesAction(delay) },
                    { "code", new CodeAction(currentIndex) },
                    { "ask-name", new AskNameAction(this) },
                    { "ask-age", new AskAgeAction(this) },
                    { "ask-email", new AskEmailAction(this) },
                    { "ask-mesa", new AskTableAction(this) },
                    { "video", new VideoAction(currentIndex) },
                    { "end", new EndAction() }
                };

                if (actions.TryGetValue(step.Type, out var action))
                {
                    action.Execute();
                    if (adminInput is -1)
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
                    sessionWriter.AddSession(session.Name, session.Age, session.Email, session.StepId, NameId);
                    helpRequest.GetHelp(NameId, session.StepId);
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


