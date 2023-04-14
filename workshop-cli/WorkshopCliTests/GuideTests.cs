using Newtonsoft.Json;
using workshop_cli;
using OfficeOpenXml;


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
            Console.WriteLine("ID:"+ step.Id);
            Console.WriteLine("Type: "+ step.Type);
            Console.WriteLine("Message: "+step.Message);
        }

    }

    [Test]
    public void TestShowExcel()
    {
        string jsonFilePath = "D:/CPDS/workshop-cli/workshop-cli/sessions.csv";
        // Read all lines from the CSV file into an array
        string[] lines = File.ReadAllLines(jsonFilePath);

        // Loop through each line in the array
        foreach (string line in lines)
        {
            // Split the line into an array of values
            string[] values = line.Split(',');

            // Print each value to the console
            foreach (string value in values)
            {
                Console.Write(value + " ");
            }

            Console.WriteLine();
        }
    }
}//hgdifkhifkjjhfilkjf