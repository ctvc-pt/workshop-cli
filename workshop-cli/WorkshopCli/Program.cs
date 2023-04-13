// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using workshop_cli;



string jsonFilePath = "C:/Users/jrafa/Desktop/REPO/joworkshop-cli/workshop-cli/Steps.json";
string jsonText = File.ReadAllText(jsonFilePath);
Console.WriteLine(jsonText);
        
var  steps = JsonConvert.DeserializeObject<List<Guide.Step>>(jsonText);
        
Guide guide = new Guide { Steps = steps };

var guideCli = new GuideCli(guide);
guideCli.Run();