using System;

namespace DisneyBroker.Models
{
    public record DisneyItem : DisneyEbayItem
    {
        public DisneyItem(string itemNo, string name, string ebayLink, int amount, bool boxIncluded, bool coaIncluded, bool isRetired, float price, float ebayPrice, DateTime scrapeDate) : base(itemNo, name, ebayLink, amount, boxIncluded, coaIncluded, isRetired, price)
        {
            EbayPrice = ebayPrice;
            ScrapeDate = scrapeDate;
        }

        public float EbayPrice { get; init; }
        public DateTime ScrapeDate { get; init; }
    }
}
