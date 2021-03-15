using DisneyBroker.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisneyBroker
{
    public class Scraper
    {
        public async Task<List<DisneyItem>> ScrapeSite(List<DisneyEbayitem> items)
        {
            List<DisneyItem> patchedItems = new List<DisneyItem>();

            foreach (DisneyItem item in items)
            {
                if (item.EbayLink == null || item.EbayLink == "")
                {
                    continue;
                }
                Console.Write("Estimate price of \"" + item.Name + "\": ");
                HttpClient webClient = new HttpClient();

                webClient.DefaultRequestHeaders.UserAgent
                    .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");

                using (HttpResponseMessage html = await webClient.GetAsync(item.EbayLink, HttpCompletionOption.ResponseContentRead))
                {
                    using (HttpContent content = html.Content)
                    {
                        string htmlString = await content.ReadAsStringAsync();

                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(htmlString);

                        HtmlNode productListHtml = htmlDocument.DocumentNode.Descendants("ul")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("srp-results srp-list clearfix")).FirstOrDefault();

                        List<HtmlNode> listItems = productListHtml.Descendants("li")
                            .Where(node => node.GetAttributeValue("class", "")
                            .StartsWith("s-item")).ToList();

                        float estimatePrice = 0;
                        double roundEstimatePrice = 0;
                        int count = 0;

                        foreach (HtmlNode listItem in listItems)
                        {
                            HtmlNode priceParent = listItem.Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("s-item__price")).FirstOrDefault();
                            HtmlNode priceElement = priceParent.Descendants("span").FirstOrDefault();

                            string price = "";
                            if (priceElement != null)
                            {
                                string innerTextTransformed = priceElement.InnerText;
                                if (innerTextTransformed.Contains("."))
                                {
                                    innerTextTransformed = innerTextTransformed.Replace(".", "");
                                }
                                price = innerTextTransformed.Replace(",", ".").Replace("EUR ", "");
                            }
                            else
                            {
                                string innerTextTransformed = priceParent.InnerText;
                                if (innerTextTransformed.Contains("."))
                                {
                                    innerTextTransformed = innerTextTransformed.Replace(".", "");
                                }
                                price = innerTextTransformed.Replace(",", ".").Replace("EUR ", "");
                            }

                            estimatePrice += Convert.ToSingle(price);
                            count++;
                        }

                        estimatePrice /= count;

                        roundEstimatePrice = Math.Round(estimatePrice, 2);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("€" + roundEstimatePrice);
                        Console.ForegroundColor = ConsoleColor.White;

                        DisneyItem test = item with
                        {
                            EbayPrice = Convert.ToSingle(roundEstimatePrice),
                            ScrapeDate = DateTime.UtcNow
                        };

                        patchedItems.Add(item);
                    }
                }
            }
            return patchedItems;
        }
    }
}
