// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using workshopCli;



   
    var filePath = Path.Combine(AppContext.BaseDirectory,"..","..","..","..","Resources","Guide","Steps.json");
    Console.WriteLine(filePath);
            
    string json = File.ReadAllText(filePath);
    var  steps = JsonConvert.DeserializeObject<List<Guide.Step>>(json);
    var guide = new Guide { Steps = steps };

    var guideCli = new GuideCli(guide);
    guideCli.Run();
