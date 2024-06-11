using Newtonsoft.Json;
using Sharprompt;
using System;
using System.Diagnostics;
using System.IO;

namespace workshopCli
{
    public class ExerciseHelper
    {
        static ProcessLuv processLuv = new ProcessLuv();

        public static string PromptAnswerAndPrint()
        {
            string sessionValue = null;

            while (sessionValue == null)
            {
                sessionValue = Prompt.Input<string>("Resposta");

                if (string.IsNullOrWhiteSpace(sessionValue))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A resposta não pode estar vazia");
                    Console.ResetColor();
                    sessionValue = null;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            return sessionValue;
        }

        public static bool PromptAnswerAndConfirm(string prompt)
        {
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));

            var chatGptClient = new ChatGptClient();
            while (true)
            {
                string wrappedString = GuideCli.WrapString(prompt, 50);
                Console.ForegroundColor = ConsoleColor.Yellow; // Set the text color to yellow for the entire prompt
                Console.WriteLine(wrappedString);
                Console.ResetColor(); // Reset text color to default

                var answer = Prompt.Input<string>("Resposta ");

                if (string.IsNullOrWhiteSpace(answer))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                    Console.ResetColor();
                    continue;
                }

                switch (answer.ToLower())
                {
                    case "ajuda":
                    case "a":
                        HandleHelp(session, chatGptClient);
                        break;
                    case "admin":
                        HandleAdmin();
                        break;
                    case "anterior":
                    case "b":
                        GuideCli.adminInput = -1;
                        return true;
                    case "proximo":
                    case "p":
                        processLuv.CloseLovecProcess();
                        return true;
                    case "reset":
                        ResetToLastCommit();
                        break;
                    case "s":
                        return false;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static void HandleHelp(Session session, ChatGptClient chatGptClient)
        {
            CsvHelpRequest.printHelp(false, true);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Qual é o problema?");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{session.Name}: ");
            var userMessage = Prompt.Input<string>("");

            var response = chatGptClient.AskGPT(userMessage).Result;
            var typewriter = new TypewriterEffect(50);
            typewriter.Type(response, ConsoleColor.Cyan);

            Console.WriteLine("\nconseguiste resolver? (sim ou não)");
            var input = Prompt.Input<string>("").ToLower();

            if (input == "sim" || input == "s")
            {
                CsvHelpRequest.printHelp(false, false);
                PromptAnswerAndConfirm(session.Name);
            }
            else if (input == "não" || input == "nao" || input == "n")
            {
                CsvHelpRequest.printHelp(true, false);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fizeste um pedido de ajuda, espera por um professor.");
                Console.WriteLine("ATENÇÃO: se saires desta mensagem o teu pedido de ajuda desaparece");
                Console.WriteLine("Escreve 'continuar' ou 'done' para continuar o workshop.");
                Console.ResetColor();

                while (true)
                {
                    var inputHelp = Prompt.Input<string>("").ToLower();
                    if (inputHelp == "continuar" || inputHelp == "done")
                    {
                        CsvHelpRequest.printHelp(false, false);
                        PromptAnswerAndConfirm(session.Name);
                    }
                    else
                    {
                        Console.WriteLine("Resposta inválida. Escreve 'continuar' ou 'done'.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Resposta inválida. Escreve 'sim' ou 'não'.");
            }
        }

        private static void HandleAdmin()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            var input = Prompt.Input<string>("").ToLower();
            var inputs = input.Split(" ");

            if (inputs.Length > 1 && inputs[0] == "id")
            {
                GuideCli.adminInput = int.Parse(inputs[1]);
            }
            else
            {
                Console.WriteLine("Resposta inválida.");
            }
        }

        private static void ResetToLastCommit()
        {
            string pythonScriptPath = Path.Combine(GuideCli.ResourcesPath, "download_last_commit.py");

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
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Código restaurado com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Erro ao restaurar o código: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar o script Python: {ex.Message}");
            }

            Console.WriteLine("");
        }
    }
}
