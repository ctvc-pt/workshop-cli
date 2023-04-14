// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using workshop_cli;

const string jsonFilePath = "D:/CPDS/workshop-cli/workshop-cli/Steps.json"; // TODO: Absolute paths are bad
var jsonText = File.ReadAllText(jsonFilePath);
Console.WriteLine(jsonText);
        
var  steps = JsonConvert.DeserializeObject<List<Guide.Step>>(jsonText);
        
var guide = new Guide { Steps = steps };

var guideCli = new GuideCli(guide);
guideCli.Run();