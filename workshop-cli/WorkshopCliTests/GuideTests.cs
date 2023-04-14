using Newtonsoft.Json;
using workshopCli;
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

        Console.WriteLine(guide.Steps[0].Id);

        var json = JsonConvert.SerializeObject(guide);

        Console.WriteLine(json);

        Assert.IsTrue(!string.IsNullOrEmpty(json));
    }

    [Test]
    public void TestDeserialization()
    {
        string jsonFilePath = "C:/Users/jrafa/RiderProjects/workshop-cli/Steps.json";
        string jsonText = File.ReadAllText(jsonFilePath);
        Console.WriteLine(jsonText);

        var steps = JsonConvert.DeserializeObject<List<Guide.Step>>(jsonText);

        Guide guide = new Guide {Steps = steps};

        foreach (Guide.Step step in guide.Steps)
        {
            Console.WriteLine("ID:" + step.Id);
            Console.WriteLine("Type: " + step.Type);
            Console.WriteLine("Message: " + step.Message);
        }

    }

    [Test]
    public void TestShowExcel()
    {
        string projectDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        string jsonFilePath = Path.Combine(projectDirectory, "sessions.csv");
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

    [Test]
    public void TestWriteExcel()
    {
        string projectDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        string csvFilePath = Path.Combine(projectDirectory, "sessions.csv");
        string txtFilePath = Path.Combine(projectDirectory, "session.txt");


    // Read the text from the sessions.txt file
        var textToAdd = File.ReadAllText(txtFilePath);

    // Deserialize the JSON text to a Session object
        var sessionCSV = JsonConvert.DeserializeObject<Session>(textToAdd);


        // Create a string representing the row to add, with each value separated by a comma
        var rowToAdd = $"{sessionCSV.Name};{sessionCSV.Age};{sessionCSV.Email};{sessionCSV.StepId}";

    // Append the row to the existing CSV file
        File.AppendAllText(csvFilePath, rowToAdd + Environment.NewLine);


        // Print the contents of the CSV file to the console
        var lines = File.ReadAllLines(csvFilePath);
        foreach (string line in lines)
        {
            // Split the line into an array of values
            var values = line.Split(',');

            // Print each value to the console
            foreach (var value in values)
            {
                Console.Write(value + "\t"); // add a tab between values for better readability
            }

            Console.WriteLine();
        }
    }

    [Test]
    public void TestUpdateExcel()
    {
        // Define the CSV file path
        string projectDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        string csvFilePath = Path.Combine(projectDirectory, "sessions.csv");

// Define the JSON file path
        string jsonFilePath = Path.Combine(projectDirectory, "sessionz.txt");

// Deserialize the JSON file contents into a Session object
        Session session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(jsonFilePath));

// Check if the session name already exists in the CSV file
        string[] lines = File.ReadAllLines(csvFilePath);
        bool found = false;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');
            if (values[0] == session.Name)
            {
                // Update the existing row with the new values
                values[1] = session.Age;
                values[2] = session.Email;
                values[3] = session.StepId.ToString();
                lines[i] = string.Join(';', values);
                found = true;
                break;
            }
        }

// If the session name was not found in the CSV file, add a new row
        if (!found)
        {
            string rowToAdd = $"{session.Name};{session.Age};{session.Email};{session.StepId}";
            File.AppendAllText(csvFilePath, rowToAdd + Environment.NewLine);
        }
        else
        {
            // Overwrite the existing CSV file with the updated contents
            File.WriteAllLines(csvFilePath, lines);
        }

        
    }

}