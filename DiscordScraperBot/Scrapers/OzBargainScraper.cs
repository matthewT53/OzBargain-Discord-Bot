using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace DiscordScraperBot.Scapers
{
    class OzBargainScraper : Scraper
    {
        /*
         * Hash table that contains the items that we scraped.
         * Key: Hash of the item title and contents.
         * Value: The corresponding item object.
         */
        private Dictionary<string, Item> _items;

        private const string URL = "https://www.ozbargain.com.au/";

        public void Scrape()
        {
            Console.Out.WriteLine("\t[+] Scraping the ozbargain website: ");

            HtmlWeb web = new HtmlWeb();

            Stack<string> links_to_follow = new Stack<string>();
            links_to_follow.Push(URL);

            while (links_to_follow.Count > 0)
            {
                string link = links_to_follow.Pop();

                var html_doc = web.Load(link);

                // Extract the desirable items from the current page:


                // Acquire the links to the next pages:
                var nodes = html_doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/ul/li");
                foreach (var node in nodes)
                {
                    
                }
            }
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
