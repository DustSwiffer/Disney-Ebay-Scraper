using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyBroker.Models
{
    public class DisneyGoogleItem
    {
        public string ItemNo { get; set; }
        public string Name { get; set; }
        public string EbayLink { get; set; }
        public int Amount { get; set; }
        public bool BoxIncluded { get; set; }
        public bool COAIncluded { get; set; }
        public bool IsRetired { get; set; }
        public float Price { get; set; }
        public float EbayPrice { get; set; }
        public DateTime ScrapeDate { get; set; }
    }
}
