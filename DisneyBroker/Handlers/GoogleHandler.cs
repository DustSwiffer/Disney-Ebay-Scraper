using DisneyBroker.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DisneyBroker.Handlers
{
    public class GoogleHandler
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Disney Broker";
        static UserCredential credential;

        public static List<DisneyItem> GetData(string sheet)
        {
            SheetsService service = ConnectToGoogle();

            String spreadsheetId = "[spreadsheet id vanuit de url]";
            String range = sheet + "!A2:k";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

         
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            List<DisneyItem> items = new List<DisneyItem>(); 
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    string priceString = row[8].ToString().Replace("€" , "");
                    DisneyItem item = new DisneyItem
                    {
                        ItemNo = (string)row[0],
                        Name = (string)row[1],
                        EbayLink = (string)row[2],
                        Amount = int.Parse(row[3].ToString()),
                        Price = Convert.ToSingle(priceString)
                    };

                    if (row[4].ToString().ToLower().Equals("yes"))
                    {
                        item.BoxIncluded = true;
                    }
                    else
                    {
                        item.BoxIncluded = false;
                    }

                    if (row[5].ToString().ToLower().Equals("yes"))
                    {
                        item.COAIncluded = true;
                    }
                    else
                    {
                        item.COAIncluded = false;
                    }

                    if (row[6].ToString().ToLower().Equals("yes"))
                    {
                        item.IsRetired = true;
                    }
                    else
                    {
                        item.IsRetired = false;
                    }

                    items.Add(item);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            return items;
        }

         private static SheetsService ConnectToGoogle()
        {
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }
    }
}
