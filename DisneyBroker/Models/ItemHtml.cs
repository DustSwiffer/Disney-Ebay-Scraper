using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyBroker.Models
{
    public class ItemHtml
    {
        public string ItemName { get; set; }
        public HtmlDocument Html { get; set; }
    }
}
