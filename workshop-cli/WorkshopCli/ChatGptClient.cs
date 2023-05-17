using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace workshopCli
{
    public class ChatGptClient
    {
        private readonly HttpClient httpClient;

        public ChatGptClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> AskGPT(string userMessage)
        {
            //----------------------
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var txtFilePath = Path.Combine( GuideCli.ResourcesPath,"session.txt" );
            var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));
            var username = session.Name;
            var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.Year}", "mygame");
            var filePath = Path.Combine(folderPath, "main.lua");
            //-----------------------
            
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", "Bearer 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");

            var requestBody = new
            {
                model = "gpt-3.5-turbo-0301",
                messages = new[]
                {
                    new { role = "system", content = "Sou um ajudante para um programa de love2d em lua, responde em portugues de portugal." + filePath +"Sempre que houver uma pergunta de codigo corrige o codigo ou mostra em que linha está o problema"},
                    new { role = "user", content = userMessage }
                }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

            var assistantMessage = responseData.choices[0].message.content;
            return assistantMessage;
        }
    }
}