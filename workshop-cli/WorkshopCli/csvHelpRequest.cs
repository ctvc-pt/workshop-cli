using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;

namespace workshopCli
{
    public class CsvHelpRequest
    {
        private readonly string csvFilePath;

        public CsvHelpRequest()
        {
            this.csvFilePath = Path.Combine( GuideCli.ResourcesPath, "sessions.csv" );
        }

        public void GetHelp( string name, string stepId )
        {
            var lines = File.ReadAllLines( csvFilePath ).ToList();
            
            for ( var i = 0; i < lines.Count; i++ )
            {
                var values = lines[ i ].Split( ';' );
                if ( values[ 0 ] != name ) continue;
                values[ 1 ] = stepId;
                lines[ i ] = string.Join( ";", values );
                File.WriteAllLines( csvFilePath, lines );


                string credentialsPath = Path.Combine( GuideCli.ResourcesPath, "client_secrets.json" );
                string spreadsheetId = "1dctnni6FLGz4OVmFI47y6bK7Vzl0SZ1G-v2N2hivWIs";
                string sheetName = "Ajudas";

                GoogleCredential credential;
                using ( var stream = new FileStream( credentialsPath, FileMode.Open, FileAccess.Read ) )
                {
                    credential = GoogleCredential.FromStream( stream )
                        .CreateScoped( SheetsService.Scope.Spreadsheets );
                }

                var service = new SheetsService( new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "WorkshopCli"
                } );

                var spreadsheet = service.Spreadsheets.Get( spreadsheetId ).Execute();
                var sheetId = GetSheetId( spreadsheet, sheetName );

                if ( sheetId != null )
                {

                    // Find the row number where values[0] matches
                    var searchRequest = service.Spreadsheets.Values.Get( spreadsheetId, $"{sheetName}!A:A" );
                    searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest
                        .ValueRenderOptionEnum
                        .UNFORMATTEDVALUE;
                    var searchResponse = searchRequest.Execute();
                    var foundRowIndex = -1;

                    if ( searchResponse.Values != null && searchResponse.Values.Count > 0 )
                    {
                        for ( int y = 0; y < searchResponse.Values.Count; y++ )
                        {
                            var rowValue = searchResponse.Values[ y ].Count > 0
                                ? searchResponse.Values[ y ][ 0 ]?.ToString()
                                : null;
                            if ( rowValue == values[ 0 ] )
                            {
                                foundRowIndex = y + 1; // Add 1 because Sheets are 1-indexed
                                break;
                            }
                        }
                    }

                    if ( foundRowIndex != -1 )
                    {
                        // Update the specific cell with new values
                        var valuesCell = new List<IList<object>>
                            { new List<object> { values[ 0 ], values[ 1 ] } };
                        var valueRange = new ValueRange { Values = valuesCell };
                        var updateRequest = service.Spreadsheets.Values.Update( valueRange, spreadsheetId,
                            $"{sheetName}!A{foundRowIndex}:B{foundRowIndex}" );
                        updateRequest.ValueInputOption =
                            SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

                        // Execute the update request
                        updateRequest.Execute();

                        Console.WriteLine( $"Updated row {foundRowIndex} in Google Sheets." );
                    }
                    else
                    {
                        // Create the append request to add a new row
                        var valuesCell = new List<IList<object>>
                            { new List<object> { values[ 0 ], values[ 1 ] } };
                        var valueRange = new ValueRange { Values = valuesCell };
                        var appendRequest =
                            service.Spreadsheets.Values.Append( valueRange, spreadsheetId, $"{sheetName}!A1:B1" );
                        appendRequest.ValueInputOption =
                            SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                        // Execute the append request
                        appendRequest.Execute();

                        Console.WriteLine( "Text file saved in Google Sheets." );
                    }
                }

                return;
            }


            //Console.WriteLine($"Name in current session {name}");
            lines.Add( $"{name};{stepId}" );
            File.WriteAllLines( csvFilePath, lines );
        }
        static int? GetSheetId( Spreadsheet spreadsheet, string sheetName )
        {
            foreach ( var sheet in spreadsheet.Sheets )
            {
                if ( sheet.Properties.Title == sheetName )
                {
                    return sheet.Properties.SheetId;
                }
            }

            return null;
        }

        public static void printHelp(bool needsHelp,bool orange)
        {
            string message;
            
            string backgroundColor;
            var assembly = Assembly.GetExecutingAssembly();
            var txtFilePath = Path.Combine( GuideCli.ResourcesPath,"session.txt" );
            
            var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));


            var name = session.Name;

            Console.WriteLine($"Name in current session {name}");

            message = needsHelp ? "needs help" : "";

            var credentialsPath = Path.Combine(GuideCli.ResourcesPath, "client_secrets.json");
            var spreadsheetId = "1dctnni6FLGz4OVmFI47y6bK7Vzl0SZ1G-v2N2hivWIs";
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
                // Find the row number where name matches
                var searchRequest = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A:A");
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
                        if (rowValue == name)
                        {
                            foundRowIndex = y + 1; // Add 1 because Sheets are 1-indexed
                            break;
                        }
                    }
                }

                if (foundRowIndex != -1)
                {
                    // Update the specific cell with new values
                    var valuesCell = new List<IList<object>>
                                    { new List<object> { message } };
                    var valueRange = new ValueRange { Values = valuesCell };
                    
                    
                    // Define the background color for cells A and B
                    var range = $"{sheetName}!A{foundRowIndex}:B{foundRowIndex}";
                    var cellFormat = new CellFormat { BackgroundColor = new Color { Red = 1.0f, Green = 1.0f, Blue = 1.0f } };
                    if (needsHelp)
                    {
                        cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.0f, Blue = 0.0f };
                    }

                    if ( orange )
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

                    // Define the background color and message for cell C
                     message = needsHelp ? "Precisa de ajuda" : "";
                     valueRange = new ValueRange { Values = valuesCell };
                    cellFormat = new CellFormat { BackgroundColor = new Color { Red = 1.0f, Green = 1.0f, Blue = 1.0f } };
                    if (needsHelp)
                    {
                        cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.0f, Blue = 0.0f };
                    }
                    if ( orange )
                    {
                        cellFormat.BackgroundColor = new Color { Red = 1.0f, Green = 0.5f, Blue = 0.0f };

                    }
                    var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId,
                        $"{sheetName}!C{foundRowIndex}");
                    updateRequest.ValueInputOption =
                        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    updateRequest.IncludeValuesInResponse = true;
                    updateRequest.Fields = "userEnteredFormat.backgroundColor";

                    // Batch update the background colors
                    var requests = new List<Request> { formatRequest, new Request { UpdateCells = new UpdateCellsRequest { Range = new GridRange { SheetId = sheetId, StartRowIndex = foundRowIndex - 1, EndRowIndex = foundRowIndex, StartColumnIndex = 2, EndColumnIndex = 3 }, Rows = new List<RowData> { new RowData { Values = new List<CellData> { new CellData { UserEnteredValue = new ExtendedValue { StringValue = message }, UserEnteredFormat = cellFormat } } } }, Fields = "userEnteredValue,userEnteredFormat.backgroundColor" } } };
                    var batchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };
                    service.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId).Execute();

                    Console.WriteLine($"Updated row {foundRowIndex} in Google Sheets with '{message}'.");
                }
                else
                {
                    Console.WriteLine($"Could not find name '{name}' in Google Sheets.");
                }
            }
        }
    }
}
