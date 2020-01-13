using System;
using System.Collections.Generic;
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
        Dictionary<string, Item> _items;

        const string URL = "https://www.ozbargain.com.au/";

        /*
         * The strings below are XPATHs for the OzBargain website. 
         * 
         * These XPATHs are used to extract desired information from the site.
         */
        const string NEXT_PAGE_LINKS_XPATH  = "//*[@id=\"content\"]/ul/li";
        const string BARGAINS_XPATH = "//*[@class=\"node node-ozbdeal node-teaser\"]";
        const string BARGAIN_TITLE_XPATH = ".//h2//a//text()";

        public void Scrape()
        {
            Console.Out.WriteLine("[+] Scraping the ozbargain website: ");

            HtmlWeb web = new HtmlWeb();

            Stack<string> links_to_follow = new Stack<string>();
            links_to_follow.Push(URL);

            while (links_to_follow.Count > 0)
            {
                string link = links_to_follow.Pop();

                var html_doc = web.Load(link);

                /*
                 * Extract all the bargains from the OzBargains website. 
                 */
                var bargain_nodes = html_doc.DocumentNode.SelectNodes(BARGAINS_XPATH);
                /* Console.Out.WriteLine("[+] Bargain nodes: ");*/
                if (bargain_nodes != null)
                {
                    Console.Out.WriteLine("[+] Bargain nodes: ");
                    /*
                     * Iterate over each product that was extracted from the ozbargain page
                     * and extract the desired information.
                     */
                    foreach (var product_node in bargain_nodes)
                    {
                        Console.Out.WriteLine("[+] Product: ");
                        /*
                         * Seems like the product title text is split up, so we need to select 
                         * an XPATH that can extract all these pieces of text.
                         */
                        var title_nodes = product_node.SelectNodes(BARGAIN_TITLE_XPATH);
                        if (title_nodes == null || title_nodes.Count == 0) continue;

                        string name = title_nodes.Count >= 1 ? title_nodes[0].InnerText : null;
                        string price = title_nodes.Count >= 2 ? title_nodes[1].InnerText : null;

                        Console.WriteLine("[+] Name: " + name);
                        Console.WriteLine("[+] Price: " + price);

                        /*
                         * Extract the link to the external source selling the product.
                         */

                    }
                }

                // Acquire the links to the next pages:
                var link_nodes = html_doc.DocumentNode.SelectNodes(NEXT_PAGE_LINKS_XPATH);
                foreach (var node in link_nodes)
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
