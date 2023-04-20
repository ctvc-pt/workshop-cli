// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Newtonsoft.Json;
using workshopCli;



   
var assembly = Assembly.GetExecutingAssembly();
var steps = new List<Guide.Step>();


using (var stream = assembly.GetManifestResourceStream("workshop_cli.Guide.Steps.json"))
using (var reader = new StreamReader(stream))
{
    var json = reader.ReadToEnd();
    steps = JsonConvert.DeserializeObject<List<Guide.Step>>(json);
}

var guide = new Guide { Steps = steps };
var guideCli = new GuideCli(guide);
guideCli.Run();




