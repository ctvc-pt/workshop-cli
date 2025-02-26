using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;

namespace workshopCli
{
    public class CsvController
    {
        private readonly string csvFilePath;

        public CsvController()
        {
            csvFilePath = Path.Combine(GuideCli.ResourcesPath, "sessions.csv");
        }

        // Método de CsvSessionWriter
        public void UpdateSession(string name, string age, string email, string stepId, string nameId, bool isChallenge = false)
        {
            var lines = File.ReadAllLines(csvFilePath).ToList();

            bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

            if (isNetworkAvailable)
            {
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

                var fullRange = $"{sheetName}!A:E";
                var getRequest = service.Spreadsheets.Values.Get(spreadsheetId, fullRange);
                getRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
                var getResponse = getRequest.Execute();

                int rowToUpdate = -1;
                int rowToInsert = 1;
                string existingStepId = "";

                if (getResponse.Values != null && getResponse.Values.Count > 0)
                {
                    for (int i = 0; i < getResponse.Values.Count; i++)
                    {
                        var row = getResponse.Values[i];
                        if (row.Count >= 5 && row[4].ToString() == nameId)
                        {
                            rowToUpdate = i + 1;
                            existingStepId = row[3].ToString();
                            break;
                        }
                    }
                    rowToInsert = getResponse.Values.Count + 1;
                }

                var spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();
                var sheetId = GetSheetId(spreadsheet, sheetName);

                if (sheetId != null)
                {
                    string stepIdToUse = isChallenge ? stepId : (rowToUpdate != -1 ? existingStepId : "");

                    var valuesCell = new List<IList<object>>
                    {
                        new List<object> { name, age, email, stepIdToUse, nameId }
                    };
                    var valueRange = new ValueRange { Values = valuesCell };

                    string range;
                    if (rowToUpdate != -1)
                    {
                        range = $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}";
                    }
                    else
                    {
                        range = $"{sheetName}!A{rowToInsert}:E{rowToInsert}";
                    }

                    var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
                    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    updateRequest.Execute();
                }
                else
                {
                    Console.WriteLine($"Sheet '{sheetName}' not found in spreadsheet.");
                }
            }
            else
            {
                Console.WriteLine("Cannot update session in Google Sheets. No internet connection. Updating CSV only.");
            }

            bool nameFound = false;
            for (var i = 0; i < lines.Count; i++)
            {
                var values = lines[i].Split(';');
                if (values[0] == name)
                {
                    values[1] = age;
                    values[2] = email;
                    values[3] = stepId;
                    values[4] = nameId;
                    lines[i] = string.Join(";", values);
                    nameFound = true;
                    break;
                }
            }

            if (!nameFound)
            {
                lines.Add($"{name};{age};{email};{stepId};{nameId}");
            }

            File.WriteAllLines(csvFilePath, lines);
        }

        // Método de CsvHelpRequest
        public void GetHelp(string name, string stepId, bool isChallenge = false)
        {
            var lines = File.ReadAllLines(csvFilePath).ToList();
            Console.WriteLine();
            for (var i = 0; i < lines.Count; i++)
            {
                var values = lines[i].Split(';');
                if (values[0] != name) continue;
                values[1] = stepId;
                lines[i] = string.Join(";", values);
                File.WriteAllLines(csvFilePath, lines);

                bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

                if (isNetworkAvailable)
                {
                    var credentialsPath = Path.Combine(GuideCli.ResourcesPath, "client_secrets.json");
                    var spreadsheetId = "1t3i31uzqSklK0R57V2AI38vWLoZPhhwADmbDtqJSKb4";
                    var sheetName = "Ajudas";

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

                    var spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();
                    var sheetId = GetSheetId(spreadsheet, sheetName);

                    if (sheetId != null)
                    {
                        var fullRange = $"{sheetName}!A:B";
                        var getRequest = service.Spreadsheets.Values.Get(spreadsheetId, fullRange);
                        getRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
                        var getResponse = getRequest.Execute();

                        int rowToUpdate = -1;
                        int rowToInsert = 1;
                        string existingStepId = "";

                        if (getResponse.Values != null && getResponse.Values.Count > 0)
                        {
                            for (int y = 0; y < getResponse.Values.Count; y++)
                            {
                                var row = getResponse.Values[y];
                                if (row.Count > 0 && row[0]?.ToString() == values[0])
                                {
                                    rowToUpdate = y + 1;
                                    existingStepId = row.Count > 1 ? row[1]?.ToString() : "";
                                    break;
                                }
                            }
                            rowToInsert = getResponse.Values.Count + 1;
                        }

                        string stepIdToUse = isChallenge ? stepId : (rowToUpdate != -1 ? existingStepId : "");

                        var valuesCell = new List<IList<object>>
                        {
                            new List<object> { values[0], stepIdToUse }
                        };
                        var valueRange = new ValueRange { Values = valuesCell };

                        if (rowToUpdate != -1)
                        {
                            var range = $"{sheetName}!A{rowToUpdate}:B{rowToUpdate}";
                            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
                            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                            updateRequest.Execute();
                        }
                        else
                        {
                            var range = $"{sheetName}!A{rowToInsert}:B{rowToInsert}";
                            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
                            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                            appendRequest.Execute();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No network connection available. Unable to update Google Sheets.");
                }

                return;
            }

            lines.Add($"{name};{stepId}");
            File.WriteAllLines(csvFilePath, lines);
        }

        // Método printHelp de CsvHelpRequest
        public static void PrintHelp(bool needsHelp, bool orange)
        {
            string message;
            var assembly = Assembly.GetExecutingAssembly();
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
            var name = session.NameId;

            bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

            if (isNetworkAvailable)
            {
                message = needsHelp ? "needs help" : "";

                var credentialsPath = Path.Combine(GuideCli.ResourcesPath, "client_secrets.json");
                var spreadsheetId = "1t3i31uzqSklK0R57V2AI38vWLoZPhhwADmbDtqJSKb4";
                var sheetName = "Ajudas";

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

                var spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();
                var sheetId = GetSheetId(spreadsheet, sheetName);

                if (sheetId != null)
                {
                    var searchRequest = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A:A");
                    searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
                    var searchResponse = searchRequest.Execute();
                    var foundRowIndex = -1;

                    if (searchResponse.Values != null && searchResponse.Values.Count > 0)
                    {
                        for (int y = 0; y < searchResponse.Values.Count; y++)
                        {
                            var rowValue = searchResponse.Values[y].Count > 0
                                ? searchResponse.Values[y][0]?.ToString()
                                : null;
                            if (rowValue == name)
                            {
                                foundRowIndex = y + 1;
                                break;
                            }
                        }
                    }

                    if (foundRowIndex != -1)
                    {
                        var valuesCell = new List<IList<object>> { new List<object> { message } };
                        var valueRange = new ValueRange { Values = valuesCell };

                        var range = $"{sheetName}!A{foundRowIndex}:B{foundRowIndex}";
                        var cellFormat = new CellFormat { BackgroundColor = new Color { Red = 1.0f, Green = 1.0f, Blue = 1.0f } };
                        if (needsHelp)
                        {
                            cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.0f, Blue = 0.0f };
                        }
                        if (orange)
                        {
                            cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.5f, Blue = 0.0f };
                        }
                        var formatRequest = new Request
                        {
                            RepeatCell = new RepeatCellRequest
                            {
                                Range = new GridRange { SheetId = sheetId, StartRowIndex = foundRowIndex - 1, EndRowIndex = foundRowIndex, StartColumnIndex = 0, EndColumnIndex = 2 },
                                Cell = new CellData { UserEnteredFormat = cellFormat },
                                Fields = "userEnteredFormat.backgroundColor"
                            }
                        };

                        message = needsHelp ? "Precisa de ajuda" : "";
                        valueRange = new ValueRange { Values = valuesCell };
                        cellFormat = new CellFormat { BackgroundColor = new Color { Red = 1.0f, Green = 1.0f, Blue = 1.0f } };
                        if (needsHelp)
                        {
                            cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.0f, Blue = 0.0f };
                        }
                        if (orange)
                        {
                            cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.5f, Blue = 0.0f };
                        }
                        var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId,
                            $"{sheetName}!C{foundRowIndex}");
                        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                        updateRequest.IncludeValuesInResponse = true;
                        updateRequest.Fields = "userEnteredFormat.backgroundColor";

                        var requests = new List<Request> { formatRequest, new Request { UpdateCells = new UpdateCellsRequest { Range = new GridRange { SheetId = sheetId, StartRowIndex = foundRowIndex - 1, EndRowIndex = foundRowIndex, StartColumnIndex = 2, EndColumnIndex = 3 }, Rows = new List<RowData> { new RowData { Values = new List<CellData> { new CellData { UserEnteredValue = new ExtendedValue { StringValue = message }, UserEnteredFormat = cellFormat } } } }, Fields = "userEnteredValue,userEnteredFormat.backgroundColor" } } };
                        var batchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };
                        service.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId).Execute();
                    }
                    else
                    {
                        Console.WriteLine($"Could not find name '{name}' in Google Sheets.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No network connection available. Unable to update Google Sheets.");
            }
        }

        // Método auxiliar compartilhado
        private static int? GetSheetId(Spreadsheet spreadsheet, string sheetName)
        {
            foreach (var sheet in spreadsheet.Sheets)
            {
                if (sheet.Properties.Title == sheetName)
                {
                    return sheet.Properties.SheetId;
                }
            }
            return null;
        }
    }
}