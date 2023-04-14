// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using workshopCli;


var guide = new Guide { Steps = JsonConvert.DeserializeObject<List<Guide.Step>>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "..", "..","..","..", "Steps.json"))) };
new GuideCli(guide).Run();
