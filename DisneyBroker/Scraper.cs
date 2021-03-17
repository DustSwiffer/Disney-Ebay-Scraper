using DisneyBroker.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DisneyBroker
{
    public class Scraper
    {
        public static List<DisneyGoogleItem> GetEstimateValueFromHtml(List<ItemHtml> itemHtmls, List<DisneyEbayItem> items)
        {
            List<DisneyGoogleItem> patchedItems = new List<DisneyGoogleItem>();

            foreach (ItemHtml itemHtml in itemHtmls)
            {
                
                DisneyEbayItem item = items.Where(i => i.Name == itemHtml.ItemName).First();
                Console.Write("Estimate value of \"" + item.Name + "\": ");
                if (itemHtml.Html == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" 0 (NO EBAY LINK)");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (item != null)
                    {
                        DisneyGoogleItem transformedEmpty = Transform(item);
                        transformedEmpty.EbayPrice = Convert.ToSingle(0);
                        transformedEmpty.ScrapeDate = DateTime.UtcNow;
                        patchedItems.Add(transformedEmpty);
                    }
                    continue;
                }
                HtmlDocument htmlDocument = itemHtml.Html;

                HtmlNode productListHtml = htmlDocument.DocumentNode.Descendants("ul")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("srp-results srp-list clearfix")).FirstOrDefault();

                List<HtmlNode> listItems = productListHtml.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .StartsWith("s-item")).ToList();

                float estimatePrice = 0;
                double roundEstimatePrice = 0;
                int count = 0;

                string[] splitItemName = itemHtml.ItemName.Split(" ");

                foreach (HtmlNode listItem in listItems)
                {
                    HtmlNode title = listItem.Descendants("h3")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Contains("s-item__title")).FirstOrDefault();
                    if(splitItemName.Any(sim => title.InnerText.Contains(sim)))
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
                        Regex priceRegex = new Regex("^(?:\\d+|\\d*\\.\\d+|(?:\\d{1,3},)?(?:\\d{3},)*\\d{3}|(?:\\d{1,3},)?(?:\\d{3},)*\\d{3}\\.\\d+)");
      
                        if (!priceRegex.IsMatch(price))
                        {
                            estimatePrice += 0;
                            count++;
                            continue;
                        }

                        estimatePrice += Convert.ToSingle(price);
                        count++;
                    }
                }

                if(estimatePrice == 0) {

                    roundEstimatePrice = 0.00;
                }
                else
                {
                    estimatePrice /= count;

                    roundEstimatePrice = Math.Round(estimatePrice, 2);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("€" + roundEstimatePrice);
                Console.ForegroundColor = ConsoleColor.White;

                if (item != null)
                {
                    DisneyGoogleItem googleItem = Transform(item);
                    googleItem.EbayPrice = Convert.ToSingle(roundEstimatePrice);
                    googleItem.ScrapeDate = DateTime.UtcNow;

                    patchedItems.Add(googleItem);
                } else
                {
                    throw new Exception("Could not find Disney item with the name " + itemHtml.ItemName);
                }
            }
            
            return patchedItems;
        }

        private static DisneyGoogleItem Transform(DisneyEbayItem ebayItem)
        {
            return new DisneyGoogleItem
            {
                ItemNo = ebayItem.ItemNo,
                Name = ebayItem.Name,
                EbayLink = ebayItem.EbayLink,
                Amount = ebayItem.Amount,
                BoxIncluded = ebayItem.BoxIncluded,
                COAIncluded = ebayItem.COAIncluded,
                IsRetired = ebayItem.IsRetired,
                Price = ebayItem.Price
            };
        }
    }
}
