using DisneyBroker.Interfaces;
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

            Console.WriteLine("");
            Console.WriteLine("Getting NON-Traditions URLs to scrape");

            GoogleClient googleClientNonTraditions = new GoogleClient("NON-Traditions");
            List<DisneyEbayItem> nonTradtions = googleClientNonTraditions.GetSheetData();

            WebCrawler webCrawlerForNonTradtions = new WebCrawler(nonTradtions);
            List<ItemHtml> NonTraditionsHtml = await webCrawlerForNonTradtions.GetHtmlAsync();

            List<DisneyGoogleItem> nTItems = Scraper.GetEstimateValueFromHtml(NonTraditionsHtml, nonTradtions);

            googleClientNonTraditions.SaveData(nTItems);

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("EBay needs a break");
            Console.WriteLine("Our script will continue within:");
            for (int seconds = 1200; seconds >= 0; seconds--)
            {
                Console.CursorLeft = 0;
                Console.Write("{0} seconds", seconds);
                Thread.Sleep(1000);
            }
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Getting Traditions URLs to scrape");

            GoogleClient googleClientTraditions = new GoogleClient("NON-Traditions");
            List<DisneyEbayItem> tradtions = googleClientTraditions.GetSheetData();

            WebCrawler webCrawlerForTradtions = new WebCrawler(tradtions);
            List<ItemHtml> TraditionsHtml = await webCrawlerForTradtions.GetHtmlAsync();

            List<DisneyGoogleItem> tItems = Scraper.GetEstimateValueFromHtml(TraditionsHtml, tradtions);

            googleClientNonTraditions.SaveData(tItems);
        }
    }
}
