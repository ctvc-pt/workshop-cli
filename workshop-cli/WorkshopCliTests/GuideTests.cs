using Newtonsoft.Json;
using workshop_cli;

namespace WorkshopCliTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSerialization()
    {
        var guide = new Guide
        {
            Steps = new List<Guide.Step>
            {
                new()
                {
                    Id = "welcome-1",
                    Type = "question",
                    Message = "Bem vindo, Qual é o teu nome?"
                }
            }
        };
        
        Console.WriteLine( guide.Steps[0].Id );

        var json = JsonConvert.SerializeObject(guide);
        
        Console.WriteLine( json );

        Assert.IsTrue( !string.IsNullOrEmpty( json ) );
    }

    [Test]
    public void TestDeserialization()
    {
        string jsonFilePath = "C:/Users/jrafa/RiderProjects/workshop-cli/Steps.json";
        string jsonText = File.ReadAllText(jsonFilePath);
        Console.WriteLine(jsonText);
        
        var  steps = JsonConvert.DeserializeObject<List<Guide.Step>>(jsonText);
        
        Guide guide = new Guide { Steps = steps };
        
        foreach (Guide.Step step in guide.Steps)
        {
            Console.WriteLine($"ID:"+ step.Id);
            Console.WriteLine("Type: "+ step.Type);
            Console.WriteLine("Message: "+step.Message);
        }

    }
}