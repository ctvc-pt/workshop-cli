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

        // Act: Update the last row in Google Sheets
        var valuesCell = new List<IList<object>>
        {
            new List<object> { values[0], values[1], values[2], values[3], values[4] }
        };
        var valueRange = new ValueRange { Values = valuesCell };
        var rowToUpdate = lines.Count; // Assuming row count matches CSV line count
        var range = $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}";
        
        var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        updateRequest.Execute();

        Console.WriteLine($"Updated row {rowToUpdate} in Google Sheets with values: {string.Join(", ", values)}");

        // Assert: Verify the update by reading the updated row
        var searchRequest = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}");
        searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
        var searchResponse = searchRequest.Execute();

        if (searchResponse.Values != null && searchResponse.Values.Count > 0)
        {
            var updatedRow = searchResponse.Values[0];
            Console.WriteLine($"Retrieved row {rowToUpdate}: {string.Join(", ", updatedRow)}");

            // Verify the values match what we sent
            Assert.AreEqual(values[0], updatedRow[0]?.ToString(), "Name mismatch");
            Assert.AreEqual(values[1], updatedRow[1]?.ToString(), "Age mismatch");
            Assert.AreEqual(values[2], updatedRow[2]?.ToString(), "Email mismatch");
            Assert.AreEqual(values[3], updatedRow[3]?.ToString(), "StepId mismatch");
            Assert.AreEqual(values[4], updatedRow[4]?.ToString(), "NameId mismatch");
        }
        else
        {
            Assert.Fail($"Failed to retrieve updated row {rowToUpdate} from Google Sheets.");
        }
    }
    
    [Test]
    public void TestWriteSpreadsheet()
    {
        CsvSessionWriter writer = new CsvSessionWriter();
        writer.UpdateSession("Luis", "22", "qewq@hgfh.com", "1", "1");
    }
}
