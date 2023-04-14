// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using workshop_cli;



    var jsonFilePath = "C:/Users/jrafa/Desktop/REPO/workshop-cli/workshop-cli/Steps.json";
    var jsonText = File.ReadAllText(jsonFilePath);
    //Console.WriteLine(jsonText);
            
    var  steps = JsonConvert.DeserializeObject<List<Guide.Step>>(jsonText);
            
    var guide = new Guide { Steps = steps };

    var guideCli = new GuideCli(guide);
    guideCli.Run();