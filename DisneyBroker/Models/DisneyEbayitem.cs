namespace DisneyBroker.Models
{
    public record DisneyEbayItem
    {
        public DisneyEbayItem(string itemNo, string name, string ebayLink, int amount, bool boxIncluded, bool coaIncluded, bool isRetired, float price)
        {
            ItemNo = itemNo;
            Name = name;
            EbayLink = ebayLink;
            Amount = amount;
            BoxIncluded = boxIncluded;
            COAIncluded = coaIncluded;
            IsRetired = isRetired;
            Price = price;
        }

        public string ItemNo { get; init; }
        public string Name { get; init; }
        public string EbayLink { get; init; }
        public int Amount { get; init; }
        public bool BoxIncluded { get; init; }
        public bool COAIncluded { get; init; }
        public bool IsRetired { get; init; }
        public float Price { get; init; }
    }
}
