using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace workshopCli
{
    public class OllamaResponse
    {
        public string Model { get; set; }
        public string Response { get; set; }
        public bool Done { get; set; }
        public long[] Context { get; set; }
        public DateTime Created_At { get; set; }
    }

    public class OllamaClient
    {
        private readonly HttpClient httpClient;

        public OllamaClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> AskOllama(string userMessage, string stepMessage)
        {
            // 1. Contexto geral da CLI (PromptChatGpt.txt)
            var filePath = Path.Combine(GuideCli.ResourcesPath, "PromptChatGpt.txt");
            var cliContext = File.ReadAllText(filePath);

            // 2. Conteúdo do main.lua (código do aluno)
            var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
            var username = session.Name;
            var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame", "main.lua");
            var studentCode = File.Exists(folderPath) ? File.ReadAllText(folderPath) : "O aluno ainda não escreveu código em main.lua.";

            // 3. Texto do passo atual (stepMessage)
            var currentStep = stepMessage;

            // 4. Pergunta do aluno (userMessage)
            var studentQuestion = userMessage;

            // Construir o prompt completo com todas as partes
            var prompt = $"### Contexto Geral da CLI ###\n{cliContext}\n\n" +
                         $"### Código do Aluno (main.lua) ###\n{studentCode}\n\n" +
                         $"### Passo Atual do Workshop ###\n{currentStep}\n\n" +
                         $"### Pergunta do Aluno ###\n{studentQuestion}";

            // Configurar a requisição para o Ollama
            var requestBody = new
            {
                model = "llama3.2:1b",
                prompt = prompt,
                stream = false
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Iniciar a tarefa de requisição ao Ollama
            Console.Write("A preparar a tua resposta ");
            Task<string> responseTask = Task.Run(async () =>
            {
                var response = await httpClient.PostAsync("http://localhost:11434/api/generate", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<OllamaResponse>(responseContent);
                return responseData.Response + "\n";
            });

            // Animação de loading (barra giratória)
            char[] spinner = new char[] { '|', '/', '-', '\\' };
            int spinnerIndex = 0;

            while (!responseTask.IsCompleted)
            {
                Console.Write($"\b{spinner[spinnerIndex]}");
                spinnerIndex = (spinnerIndex + 1) % spinner.Length; 
                await Task.Delay(200); 
            }

            // Limpar a linha de loading e retornar a resposta
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            string finalResponse = await responseTask;
            return finalResponse;
        }
    }
}