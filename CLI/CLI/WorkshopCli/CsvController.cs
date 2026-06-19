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
    public enum HelpState
    {
        None,
        Pending,
        Resolved,
        NeedsTeacher
    }

    public class CsvController
    {
        readonly string csvFilePath;
        const string SpreadsheetId = "1njSnYweZnBLdnoUnJjJFSO73pb7huFEOxt2J3Bc4BYw";

        private static readonly Lazy<SheetsService> _service = new( BuildService );

        public CsvController()
        {
            csvFilePath = Path.Combine( GuideCli.ResourcesPath, "sessions.csv" );
        }

        private static SheetsService BuildService()
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

            if ( !NetworkInterface.GetIsNetworkAvailable() )
            {
                return;
            }

            try
            {
                var sheetName = "Sessions";
                var service = _service.Value;

                var fullRange = $"{sheetName}!A:F";
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
                    string timestamp = DateTime.Now.ToString( "o" );

                    var valuesCell = new List<IList<object>>
                    {
                        new List<object> { name, age, email, stepIdToUse, nameId, timestamp }
                    };
                    var valueRange = new ValueRange { Values = valuesCell };

                    string range;
                    if ( rowToUpdate != -1 )
                    {
                        range = $"{sheetName}!A{rowToUpdate}:F{rowToUpdate}";
                    }
                    else
                    {
                        range = $"{sheetName}!A{rowToInsert}:F{rowToInsert}";
                    }

                    var updateRequest = service.Spreadsheets.Values.Update( valueRange, SpreadsheetId, range );
                    updateRequest.ValueInputOption =
                        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    updateRequest.Execute();

                    AppendTimeline( service, nameId, stepId, timestamp );
                }
            }
            catch
            {
                // Keep the student flow clean. The local CSV was already updated above.
            }
        }

        // Append-only history para analytics: cada transicao de step gera uma linha
        // (nameId, stepId, timestamp). A sheet "Timeline" tem de existir no spreadsheet.
        private static void AppendTimeline( SheetsService service, string nameId, string stepId, string timestamp )
        {
            try
            {
                var values = new List<IList<object>>
                {
                    new List<object> { nameId, stepId, timestamp }
                };
                var append = service.Spreadsheets.Values.Append(
                    new ValueRange { Values = values },
                    SpreadsheetId,
                    "Timeline!A:C" );
                append.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
                append.Execute();
            }
            catch
            {
                // Se a sheet Timeline nao existir ou o append falhar, nao bloqueamos a sessao do miudo.
            }
        }

        // Sheet "Ajudas" e append-only com timestamp:
        //   A=nameId, B=stepId, C=status ("", "Resolvido", "Precisa de ajuda"), D=timestamp ISO
        //
        // Pending  -> APPEND nova linha + pinta laranja (cada "ajuda" do miudo = 1 linha)
        // Resolved -> UPDATE da linha mais recente do miudo + pinta verde
        // NeedsTeacher -> UPDATE da linha mais recente do miudo + pinta vermelho
        public static void PrintHelp( HelpState state )
        {
            var txtFilePath = Path.Combine( GuideCli.ResourcesPath, "session.txt" );
            var session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
            var name = session.NameId;
            var stepId = session.StepId ?? "";

            if ( !NetworkInterface.GetIsNetworkAvailable() )
            {
                Console.WriteLine( "No network connection available. Unable to update Google Sheets." );
                return;
            }

            var (bgColor, message) = GetStateStyle( state );

            var sheetName = "Ajudas";
            var service = _service.Value;

            var spreadsheet = service.Spreadsheets.Get( SpreadsheetId ).Execute();
            var sheetId = GetSheetId( spreadsheet, sheetName );
            if ( sheetId == null ) return;

            // Pending = pedido novo: append uma linha vazia (sera pintada e preenchida em baixo).
            if ( state == HelpState.Pending )
            {
                string timestamp = DateTime.Now.ToString( "o" );
                var newRow = new List<IList<object>>
                {
                    new List<object> { name, stepId, "", timestamp }
                };
                var appendRequest = service.Spreadsheets.Values.Append(
                    new ValueRange { Values = newRow },
                    SpreadsheetId,
                    $"{sheetName}!A:D" );
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest
                    .ValueInputOptionEnum.RAW;
                appendRequest.Execute();
            }

            var searchRequest = service.Spreadsheets.Values.Get( SpreadsheetId, $"{sheetName}!A:A" );
            searchRequest.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest
                .ValueRenderOptionEnum.UNFORMATTEDVALUE;
            var searchResponse = searchRequest.Execute();
            var foundRowIndex = -1;

            // Procuramos a linha mais recente do miudo (de baixo para cima — ordem de append).
            if ( searchResponse.Values != null && searchResponse.Values.Count > 0 )
            {
                for ( int y = searchResponse.Values.Count - 1; y >= 0; y-- )
                {
                    var rowValue = searchResponse.Values[ y ].Count > 0
                        ? searchResponse.Values[ y ][ 0 ]?.ToString()
                        : null;
                    if ( rowValue == name )
                    {
                        foundRowIndex = y + 1;
                        break;
                    }
                }
            }

            if ( foundRowIndex == -1 ) return;

            var cellFormat = new CellFormat { BackgroundColor = bgColor };

            var requests = new List<Request>
            {
                new Request
                {
                    RepeatCell = new RepeatCellRequest
                    {
                        Range = new GridRange
                        {
                            SheetId = sheetId, StartRowIndex = foundRowIndex - 1, EndRowIndex = foundRowIndex,
                            StartColumnIndex = 0, EndColumnIndex = 2
                        },
                        Cell = new CellData { UserEnteredFormat = cellFormat },
                        Fields = "userEnteredFormat.backgroundColor"
                    }
                },
                new Request
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Range = new GridRange
                        {
                            SheetId = sheetId, StartRowIndex = foundRowIndex - 1,
                            EndRowIndex = foundRowIndex, StartColumnIndex = 2, EndColumnIndex = 3
                        },
                        Rows = new List<RowData>
                        {
                            new RowData
                            {
                                Values = new List<CellData>
                                {
                                    new CellData
                                    {
                                        UserEnteredValue = new ExtendedValue { StringValue = message },
                                        UserEnteredFormat = cellFormat
                                    }
                                }
                            }
                        },
                        Fields = "userEnteredValue,userEnteredFormat.backgroundColor"
                    }
                }
            };

            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };
            service.Spreadsheets.BatchUpdate( batchUpdateRequest, SpreadsheetId ).Execute();
        }

        static (Color color, string message) GetStateStyle( HelpState state ) => state switch
        {
            HelpState.Pending      => (new Color { Red = 1.0f, Green = 0.5f, Blue = 0.0f }, ""),
            HelpState.Resolved     => (new Color { Red = 0.0f, Green = 1.0f, Blue = 0.0f }, "Resolvido"),
            HelpState.NeedsTeacher => (new Color { Red = 1.0f, Green = 0.0f, Blue = 0.0f }, "Precisa de ajuda"),
            _                      => (new Color { Red = 1.0f, Green = 1.0f, Blue = 1.0f }, ""),
        };

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
