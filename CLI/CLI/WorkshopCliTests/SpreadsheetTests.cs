using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using workshopCli;

namespace WorkshopCliTests;

public class SpreadsheetTests
{
    private string csvFilePath;
    
    [SetUp]
    public void Setup()
    {
        csvFilePath = Path.Combine(GuideCli.ResourcesPath, "sessions.csv");
    }

    [Test]
    public void TestGetSpreadsheet()
    {
        var lines = File.ReadAllLines(csvFilePath).ToList();
        string[] values = new string[] { };
        
        for (var i = 0; i < lines.Count; i++)
        {
            values = lines[i].Split(';');
        }

        var credentialsPath = Path.Combine(GuideCli.ResourcesPath, "client_secrets.json");
        var spreadsheetId = "1t3i31uzqSklK0R57V2AI38vWLoZPhhwADmbDtqJSKb4";
        var sheetName = "Sessions";
        
        GoogleCredential credential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(SheetsService.Scope.Spreadsheets);
        }
        
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "WorkshopCli"
        });
        
        // Find the row number where values[0] matches
        var searchRequest = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!E:E");
        searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest
            .ValueRenderOptionEnum.UNFORMATTEDVALUE;
        var searchResponse = searchRequest.Execute();
        var foundRowIndex = -1;

        if (searchResponse.Values != null && searchResponse.Values.Count > 0)
        {
            for (int y = 0; y < searchResponse.Values.Count; y++)
            {
                var rowValue = searchResponse.Values[y].Count > 0
                    ? searchResponse.Values[y][0]?.ToString()
                    : null;

                if (!string.IsNullOrEmpty(rowValue))
                {
                    Console.WriteLine(rowValue);
                }
                
                if (rowValue == values[4])
                {
                    foundRowIndex = y + 1; // Add 1 because Sheets are 1-indexed
                    break;
                }
            }
        }
    }
    
    [Test]
    public void TestSimultaneousSpreadsheetUpdates()
    {
        for ( int i = 0; i < 1000; i++ )
        {
            UpdateSpreadsheet( "Teste", "10", "test@test.com", "10", i.ToString() );
        }
    }

    async Task UpdateSpreadsheet(string name, string age, string email, string stepId, string nameId)
    {
        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " - Starting update");
        var csvController = new CsvController();
        csvController.UpdateSession( name, age, email, stepId, nameId, true );
    }
    
    [Test]
public void TestUpdateSpreadsheet()
{
    // Arrange: Read the CSV and get the last line
    var lines = File.ReadAllLines(csvFilePath).ToList();
    if (lines.Count == 0)
    {
        Assert.Fail("CSV file is empty. Please ensure it has at least one line for testing.");
        return;
    }

    // Get the values from the last line
    var lastLine = lines[lines.Count - 1];
    var values = lastLine.Split(';');
    if (values.Length < 5)
    {
        Assert.Fail("Last line in CSV does not have enough values (expected 5).");
        return;
    }
    
    // Google Sheets setup
    var credentialsPath = Path.Combine(GuideCli.ResourcesPath, "client_secrets.json");
    var spreadsheetId = "1t3i31uzqSklK0R57V2AI38vWLoZPhhwADmbDtqJSKb4";
    var sheetName = "Sessions";

    GoogleCredential credential;
    using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
    {
        credential = GoogleCredential.FromStream(stream)
            .CreateScoped(SheetsService.Scope.Spreadsheets);
    }

    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "WorkshopCli"
    });

    // Obter todos os dados da spreadsheet primeiro
    var fullRange = $"{sheetName}!A:E";
    var getRequest = service.Spreadsheets.Values.Get(spreadsheetId, fullRange);
    getRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
    var getResponse = getRequest.Execute();

    int rowToUpdate;
    if (getResponse.Values != null && getResponse.Values.Count > 0)
    {
        Console.WriteLine($"Retrieved {getResponse.Values.Count} rows from Google Sheets:");
        foreach (var row in getResponse.Values)
        {
            Console.WriteLine(string.Join(", ", row));
        }
        // Última linha ocupada + 1 será a próxima linha disponível
        rowToUpdate = getResponse.Values.Count + 1;
    }
    else
    {
        Console.WriteLine("No data found in the spreadsheet.");
        // Se não há dados, começamos na linha 1
        rowToUpdate = 1;
    }

    // Act: Inserir os dados do CSV na próxima linha disponível
    var valuesCell = new List<IList<object>>
    {
        new List<object> { values[0], values[1], values[2], values[3], values[4] }
    };
    var valueRange = new ValueRange { Values = valuesCell };
    var range = $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}";

    var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
    updateRequest.Execute();

    Console.WriteLine($"Inserted row {rowToUpdate} in Google Sheets with values: {string.Join(", ", values)}");

    // Assert: Verify the insertion by reading the new row
    var searchRequest = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}");
    searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
    var searchResponse = searchRequest.Execute();

    if (searchResponse.Values != null && searchResponse.Values.Count > 0)
    {
        var insertedRow = searchResponse.Values[0];
        Console.WriteLine($"Retrieved row {rowToUpdate}: {string.Join(", ", insertedRow)}");

        // Verify the values match what we sent from the CSV
        Assert.AreEqual(values[0], insertedRow[0]?.ToString(), "Name mismatch");
        Assert.AreEqual(values[1], insertedRow[1]?.ToString(), "Age mismatch");
        Assert.AreEqual(values[2], insertedRow[2]?.ToString(), "Email mismatch");
        Assert.AreEqual(values[3], insertedRow[3]?.ToString(), "StepId mismatch");
        Assert.AreEqual(values[4], insertedRow[4]?.ToString(), "NameId mismatch");
    }
    else
    {
        Assert.Fail($"Failed to retrieve inserted row {rowToUpdate} from Google Sheets.");
    }
}
}
