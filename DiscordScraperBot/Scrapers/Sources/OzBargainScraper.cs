using System;
using System.Collections.Generic;
using System.Linq;
using DiscordScraperBot.BotMessages;
using HtmlAgilityPack;
using LLibrary;

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

            var base_html = web.Load(BaseUrl);
            List<string> links_to_follow = ExtractNextLinks(base_html);
            links_to_follow.Add(BaseUrl);

            foreach (string link in links_to_follow)
            {
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

            string name  = "";
            string price = "";
            try
            {
                name = title_nodes.Count >= 1 ? title_nodes[0].InnerText : null;
                price = title_nodes.Count >= 2 ? title_nodes[1].InnerText : null;
            }

            catch (Exception e)
            {
                Logger logger = Logger.GetInstance();
                logger.realLogger.Error(e);
            }

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

            string externalUrl = "";
            string imageUrl = "";
            try
            {
                externalUrl = BaseUrl + right_nodes[0].Attributes["href"].Value;
                imageUrl = right_nodes[0].FirstChild.Attributes["src"].Value;
            }

            catch (Exception e)
            {
                Logger logger = Logger.GetInstance();
                logger.realLogger.Error(e);
            }
            
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
        private List<string> ExtractNextLinks(HtmlDocument htmlDoc)
        {
            List<string> nextLinks = new List<string>();
            var unorderedList = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"main\"]/ul");

            foreach (var listItem in unorderedList.ChildNodes)
            {
                if ( IsLinkRelevant(listItem) )
                {
                    //TODO Skip duplicates
                    var linkItem = listItem.FirstChild;
                    var link = BaseUrl + linkItem.Attributes["href"].Value;
                    nextLinks.Add(link);
                    Console.WriteLine("[+] Child class: " + link);
                }
            }

            return nextLinks;
        }

        private bool IsLinkRelevant(HtmlNode listItem)
        {
            return listItem.GetAttributeValue("class", "empty").Equals("empty");
        }
    }
}
