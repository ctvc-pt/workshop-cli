using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

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
    }
}
