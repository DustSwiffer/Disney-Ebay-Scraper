using DisneyBroker.Handlers;
using DisneyBroker.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DisneyBroker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@" ____________________________________________________________________________________________________________");
            Console.WriteLine(@"/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"                         ____  _                          ____             __            ");
            Console.WriteLine(@"                        / __ \(_)________  ___  __  __   / __ )_________  / /_____  _____");
            Console.WriteLine(@"                       / / / / / ___/ __ \/ _ \/ / / /  / __  / ___/ __ \/ //_/ _ \/ ___/");
            Console.WriteLine(@"                      / /_/ / (__  ) / / /  __/ /_/ /  / /_/ / /  / /_/ / ,< /  __/ /    ");
            Console.WriteLine(@"                     /_____/_/____/_/ /_/\___/\__, /  /_____/_/   \____/_/|_|\___/_/     ");
            Console.WriteLine(@"                                             /____/                                   ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@" ____________________________________________________________________________________________________________");
            Console.WriteLine(@"/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Webscraping eBay to get the estimate value of the Disney product");

            var scraper = new Scraper();

            Console.WriteLine("");
            Console.WriteLine("Getting NON-Traditions URLs to scrape");
            List<DisneyItem> nonTradtions = GoogleHandler.GetData("NON-Traditions");

            List<DisneyItem> modifiedNonTraditions = await scraper.ScrapeSite(nonTradtions);

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("EBay needs a break");
            Console.WriteLine("Our script will continue within:");
            for (int seconds = 1200; seconds >= 0; seconds--)
            {
                Console.CursorLeft = 0;
                Console.Write("{0} seconds", seconds);    // Add space to make sure to override previous contents
                Thread.Sleep(1000);
            }
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Getting Traditions URLs to scrape");

            List<DisneyItem> tradtions = GoogleHandler.GetData("Traditions");

            List<DisneyItem> modifiedTraditions = await scraper.ScrapeSite(tradtions);
            Console.WriteLine(modifiedTraditions.ToString());
        }
    }
}
