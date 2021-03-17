using HtmlAgilityPack;

namespace DisneyBroker.Models
{
    public class ItemHtml
    {
        public string ItemName { get; set; }
        public HtmlDocument Html { get; set; }
    }
}
