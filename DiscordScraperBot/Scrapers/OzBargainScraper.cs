using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace DiscordScraperBot.Scapers
{
    class OzBargainScraper : Scraper
    {
        private static string URL = "https://www.ozbargain.com.au/";
        public void Scrape()
        {
            Console.Out.WriteLine("\t[+] Scraping the ozbargain website: ");

            HtmlWeb web = new HtmlWeb();

            var html_doc = web.Load(URL);


        }

        public List<Item> GetItems()
        {
            return null;
        }

        public void ClearItems()
        {

        }
    }
}
