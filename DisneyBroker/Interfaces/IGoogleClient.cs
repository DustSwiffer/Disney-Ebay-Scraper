using DisneyBroker.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace DisneyBroker.Interfaces
{
    interface IGoogleClient
    {
        string sheet{ get; }
        List<DisneyEbayItem> GetSheetData();
    }

    class GoogleClient : IGoogleClient
    {
        public string sheet { get; set; }

        private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private static string ApplicationName = "Disney Broker";
        private static UserCredential credential;
        public GoogleClient(string _sheet)
        {
            sheet = _sheet;
        }

        public List<DisneyEbayItem> GetSheetData()
        {
            SheetsService service = ConnectToGoogle();

            string spreadsheetId = ConfigurationManager.AppSettings["GoogleSheetId"];
            string range = sheet + "!A2:k";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);


            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            List<DisneyEbayItem> items = new List<DisneyEbayItem>();
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    string priceString = row[8].ToString().Replace("€", "");
                    DisneyEbayItem item = new DisneyEbayItem
                    (
                        itemNo: (string)row[0],
                        name: (string)row[1],
                        ebayLink: (string)row[2],
                        amount: int.Parse(row[3].ToString()),
                        price: Convert.ToSingle(priceString),
                        boxIncluded: row[4].ToString().ToLower().Equals("yes"),
                        coaIncluded: row[5].ToString().ToLower().Equals("yes"),
                        isRetired: row[6].ToString().ToLower().Equals("yes")
                    );

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
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }
    }
}
