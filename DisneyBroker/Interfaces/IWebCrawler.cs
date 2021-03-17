using DisneyBroker.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisneyBroker.Interfaces
{
    interface IWebCrawler
    {
        public List<DisneyEbayItem> items { get; set; }

        Task<List<ItemHtml>> GetHtmlAsync();
    }

    class WebCrawler : IWebCrawler
    {
        public List<DisneyEbayItem> items { get; set; }
        public WebCrawler(List<DisneyEbayItem> _items)
        {
            items = _items; 
        }

        public async Task<List<ItemHtml>> GetHtmlAsync()
        {
            List<ItemHtml> htmlList = new List<ItemHtml>();
            foreach (DisneyEbayItem item  in items)
            {
                if (item.EbayLink == null || item.EbayLink == "")
                {
                    continue;
                }

                HttpClient httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.UserAgent
                    .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");

                using (HttpResponseMessage response = await httpClient.GetAsync(item.EbayLink, HttpCompletionOption.ResponseContentRead))
                {
                    using(HttpContent content = response.Content)
                    {
                        string htmlString = await content.ReadAsStringAsync();

                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(htmlString);

                        ItemHtml itemHtml = new ItemHtml
                        {
                            ItemName = item.Name,
                            Html = htmlDocument
                        };
                        htmlList.Add(itemHtml);
                    }
                }
            }
            return htmlList;
        }
    }
}
