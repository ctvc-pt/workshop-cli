namespace workshopCli;

public class CsvSessionWriter
{
    private string csvFilePath;

    
    public CsvSessionWriter()
    {
        csvFilePath = Path.Combine(AppContext.BaseDirectory,"..","..","..","..","..","Resources", "sessions.csv");
    }

    public void AddSession(string name, string age, string email, string stepId)
    {
        var lines = File.ReadAllLines(csvFilePath).ToList();

        for (var i = 0; i < lines.Count; i++)
        {
            var values = lines[i].Split(';');
            if (values[0] != name) continue;
            values[1] = age;
            values[2] = email;
            values[3] = stepId;
            lines[i] = string.Join(";", values);
            File.WriteAllLines(csvFilePath, lines);
            return;
        }

        lines.Add($"{name};{age};{email};{stepId}");
        File.WriteAllLines(csvFilePath, lines);
    }
}