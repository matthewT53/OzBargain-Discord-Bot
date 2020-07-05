using System;
using System.Collections.Generic;
using DiscordScraperBot.BotMessages;
using HtmlAgilityPack;

namespace DiscordScraperBot.Scapers
{
    class OzBargainScraper : Scraper
    {
        const string BaseUrl = "https://www.ozbargain.com.au/";

        /*
         * The strings below are XPATHs for the OzBargain website. 
         * 
         * These XPATHs are used to extract desired information from the site.
         */
        const string NextPageLinksXpath  = "//*[@id=\"content\"]/ul/li";
        const string BargainsXpath = "//*[@class=\"node node-ozbdeal node-teaser\"]";
        const string BargainTitleXPath = ".//h2//a//text()";

        public OzBargainScraper(ulong channelId)
            : base(channelId)
        { 
        }

        /***
         * This method scrapes all the products that are on sale from the ozbargain website. 
         * These products are stored in a list of IBotMessages.
         */
        public override void Scrape()
        {
            Console.Out.WriteLine("[+] Scraping the ozbargain website: ");

            HtmlWeb web = new HtmlWeb();

            Stack<string> links_to_follow = new Stack<string>();
            links_to_follow.Push(BaseUrl);

            while (links_to_follow.Count > 0)
            {
                string link = links_to_follow.Pop();
                var html_doc = web.Load(link);

                /*
                 * Extract all the bargains from the OzBargains website. 
                 */
                var bargain_nodes = html_doc.DocumentNode.SelectNodes(BargainsXpath);
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
                        IBotMessage message = ExtractProductInfo(product_node);
                        if (message != null)
                        {
                            base.AddMessage(message);
                        }
                    }
                }
            }
        }

        /***
         * This method extracts information about a product found on the OzBargain website.
         * Input:
         *  - HtmlNode object.
         * Returns a BargainMessage object with the extracted inforamtion. 
         */
        private BargainMessage ExtractProductInfo(HtmlNode product_node)
        {
            Console.Out.WriteLine("[+] Product: ");
            /*
             * Seems like the product title text is split up, so we need to select 
             * an XPATH that can extract all these pieces of text.
             */
            var title_nodes = product_node.SelectNodes(BargainTitleXPath);
            if (title_nodes == null || title_nodes.Count == 0)
            {
                return null;
            }

            // TODO: Add exception handling code incase title_nodes[0] = null
            string name = title_nodes.Count >= 1 ? title_nodes[0].InnerText : null;
            string price = title_nodes.Count >= 2 ? title_nodes[1].InnerText : null;

            Console.WriteLine("[+] Name: " + name);
            Console.WriteLine("[+] Price: " + price);

            /*
             * Extract the link to the external source selling the product.
             */
            var right_nodes = product_node.SelectNodes(".//div[2]//div[1]//div//a");
            if (right_nodes == null || right_nodes.Count == 0)
            {
                return null;
            }

            string externalUrl = BaseUrl + right_nodes[0].Attributes["href"].Value;
            string imageUrl = right_nodes[0].FirstChild.Attributes["src"].Value;
            Console.WriteLine("[+] externalUrl: " + externalUrl);
            Console.WriteLine("[+] imageUrl: " + imageUrl);

            BargainMessage message = new BargainMessage(name, price, externalUrl, imageUrl);
            return message;
        }

        /***
         * This method extracts the URLs that lead to the next pages.
         * Input:
         * - HtmlDocument object 
         * Returns a list of URLs that lead to the next set of pages on the ozbargain website.
         */
        private Stack<string> ExtractNextLinks(HtmlDocument html_doc)
        {
            //TODO:
            Stack<string> next_links = new Stack<string>();
            return next_links;
        }
    }
}
