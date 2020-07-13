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
        const string BaseUrl = "https://www.ozbargain.com.au";

        const string BargainsXpath = "//*[@class=\"node node-ozbdeal node-teaser\"]";
        const string BargainTitleXPath = ".//h2//a//text()";
        const string BargainExternalLinkXPath = ".//div[2]//div[1]//div//a";
        const string ListOfNextPagesXPath = "//*[@id=\"main\"]/ul";

        const string ScraperName = "OzBargainScraper";

        public OzBargainScraper(Preferences preferences)
            : base(preferences, ScraperName)
        { 
        }

        /***
         * This method scrapes all the products that are on sale from the ozbargain website. 
         * These products are stored in a list of IBotMessages.
         */
        public override void Scrape()
        {
            HtmlWeb web = new HtmlWeb();

            var baseHtml = web.Load(BaseUrl);
            HashSet<string> linksToFollow = ExtractNextLinks(baseHtml);
            linksToFollow.Add(BaseUrl);

            int currentDepth = 1;
            foreach (string link in linksToFollow)
            {
                Console.WriteLine("[+] Link: " + link);
                var htmlDoc = web.Load(link);

                /*
                 * Extract all the bargains from the OzBargains website. 
                 */
                var bargainNodes = htmlDoc.DocumentNode.SelectNodes(BargainsXpath);
                if (bargainNodes != null)
                {
                    Console.Out.WriteLine("[+] Bargain nodes: ");
                    foreach (var productNode in bargainNodes)
                    {
                        IBotMessage message = ExtractProductInfo(productNode);
                        if (message != null)
                        {
                            AddMessage(message);
                        }
                    }
                }

                if (currentDepth >= GetDepth())
                {
                    break;
                }
            }
        }

        public override string GetName()
        {
            return ScraperName;
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
        
            string name = "";
            string price = "";
            string externalUrl = "";
            string imageUrl = "";

            try
            {
                var title_nodes = product_node.SelectNodes(BargainTitleXPath);
                if (title_nodes == null || title_nodes.Count == 0)
                {
                    return null;
                }

                name = title_nodes.Count >= 1 ? title_nodes[0].InnerText : null;
                price = title_nodes.Count >= 2 ? title_nodes[1].InnerText : null;

                Console.WriteLine("[+] Name: " + name);
                Console.WriteLine("[+] Price: " + price);

                var right_nodes = product_node.SelectNodes(BargainExternalLinkXPath);
                if (right_nodes == null || right_nodes.Count == 0)
                {
                    return null;
                }

                externalUrl = BaseUrl + right_nodes[0].Attributes["href"].Value;
                imageUrl = right_nodes[0].FirstChild.Attributes["src"].Value;

                Console.WriteLine("[+] externalUrl: " + externalUrl);
                Console.WriteLine("[+] imageUrl: " + imageUrl);
            }

            catch (Exception e)
            {
                Logger logger = Logger.GetInstance();
                logger.realLogger.Error(e);
            }

            BargainMessage message = new BargainMessage(name, price, externalUrl, imageUrl);
            return message;
        }

        /***
         * This method extracts the URLs that lead to the next pages.
         * Input:
         * - HtmlDocument object 
         * Returns a set of URLs that lead to the next set of pages on the ozbargain website.
         */
        private HashSet<string> ExtractNextLinks(HtmlDocument htmlDoc)
        {
            HashSet<string> nextLinks = new HashSet<string>();
            var unorderedList = htmlDoc.DocumentNode.SelectSingleNode(ListOfNextPagesXPath);

            foreach (var listItem in unorderedList.ChildNodes)
            {
                if ( IsLinkRelevant(listItem) )
                {
                    var linkItem = listItem.FirstChild;
                    var link = BaseUrl + linkItem.Attributes["href"].Value;
                    nextLinks.Add(link);
                }
            }

            return nextLinks;
        }

        /***
         * Determines if the <li> item that contains a link is required.
         */
        private bool IsLinkRelevant(HtmlNode listItem)
        {
            return listItem.GetAttributeValue("class", "empty").Equals("empty");
        }
    }
}
