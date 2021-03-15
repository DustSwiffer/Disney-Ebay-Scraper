using DisneyBroker.Handlers;
using DisneyBroker.Models;
using DisneyBroker.Utilities;
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

            ConsoleBannerWriter.Write();

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
