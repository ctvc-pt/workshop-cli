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
        readonly string csvFilePath;
        const string SpreadsheetId = "1t3i31uzqSklK0R57V2AI38vWLoZPhhwADmbDtqJSKb4";

        public CsvController()
        {
            csvFilePath = Path.Combine( GuideCli.ResourcesPath, "sessions.csv" );
        }

        SheetsService GetSheetsService()
        {
            var credentialsPath = Path.Combine( GuideCli.ResourcesPath, "client_secrets.json" );

            GoogleCredential credential;
            using ( var stream = new FileStream( credentialsPath, FileMode.Open, FileAccess.Read ) )
            {
                credential = GoogleCredential.FromStream( stream )
                    .CreateScoped( SheetsService.Scope.Spreadsheets );
            }

            return new SheetsService( new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "WorkshopCli"
            } );
        }

        public void UpdateSession( string name, string age, string email, string stepId, string nameId,
            bool isChallenge = false )
        {
            var lines = File.ReadAllLines( csvFilePath ).ToList();

            bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

            if ( isNetworkAvailable )
            {
                var sheetName = "Sessions";
                var service = GetSheetsService();

                var fullRange = $"{sheetName}!A:E";
                var getRequest = service.Spreadsheets.Values.Get( SpreadsheetId, fullRange );
                getRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum
                    .UNFORMATTEDVALUE;
                var getResponse = getRequest.Execute();

                int rowToUpdate = -1;
                int rowToInsert = 1;
                string existingStepId = "";

                if ( getResponse.Values != null && getResponse.Values.Count > 0 )
                {
                    for ( int i = 0; i < getResponse.Values.Count; i++ )
                    {
                        var row = getResponse.Values[ i ];
                        if ( row.Count >= 5 && row[ 4 ].ToString() == nameId )
                        {
                            rowToUpdate = i + 1;
                            existingStepId = row[ 3 ].ToString();
                            break;
                        }
                    }

                    rowToInsert = getResponse.Values.Count + 1;
                }

                var spreadsheet = service.Spreadsheets.Get( SpreadsheetId ).Execute();
                var sheetId = GetSheetId( spreadsheet, sheetName );

                if ( sheetId != null )
                {
                    string stepIdToUse = isChallenge ? stepId : ( rowToUpdate != -1 ? existingStepId : "" );

                    var valuesCell = new List<IList<object>>
                    {
                        new List<object> { name, age, email, stepIdToUse, nameId }
                    };
                    var valueRange = new ValueRange { Values = valuesCell };

                    string range;
                    if ( rowToUpdate != -1 )
                    {
                        range = $"{sheetName}!A{rowToUpdate}:E{rowToUpdate}";
                    }
                    else
                    {
                        range = $"{sheetName}!A{rowToInsert}:E{rowToInsert}";
                    }

                    var updateRequest = service.Spreadsheets.Values.Update( valueRange, SpreadsheetId, range );
                    updateRequest.ValueInputOption =
                        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    updateRequest.Execute();
                }
            }
            else
            {
                Console.WriteLine(
                    "Cannot update session in Google Sheets. No internet connection. Updating CSV only." );
            }

            bool nameFound = false;
            for ( var i = 0; i < lines.Count; i++ )
            {
                var values = lines[ i ].Split( ';' );
                if ( values[ 0 ] == name )
                {
                    values[ 1 ] = age;
                    values[ 2 ] = email;
                    values[ 3 ] = stepId;
                    values[ 4 ] = nameId;
                    lines[ i ] = string.Join( ";", values );
                    nameFound = true;
                    break;
                }
            }

            if ( !nameFound )
            {
                lines.Add( $"{name};{age};{email};{stepId};{nameId}" );
            }

            File.WriteAllLines( csvFilePath, lines );
        }

        private static int? GetSheetId( Spreadsheet spreadsheet, string sheetName )
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
