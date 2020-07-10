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

        const string NextPageLinksXpath  = "//*[@id=\"content\"]/ul/li";
        const string BargainsXpath = "//*[@class=\"node node-ozbdeal node-teaser\"]";
        const string BargainTitleXPath = ".//h2//a//text()";
        const string BargainExternalLinkXPath = ".//div[2]//div[1]//div//a";
        const string ListOfNextPagesXPath = "//*[@id=\"main\"]/ul";

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
            HtmlWeb web = new HtmlWeb();

            var base_html = web.Load(BaseUrl);
            HashSet<string> links_to_follow = ExtractNextLinks(base_html);
            links_to_follow.Add(BaseUrl);

            foreach (string link in links_to_follow)
            {
                Console.WriteLine("[+] Link: " + link);
                var html_doc = web.Load(link);

                /*
                 * Extract all the bargains from the OzBargains website. 
                 */
                var bargain_nodes = html_doc.DocumentNode.SelectNodes(BargainsXpath);
                if (bargain_nodes != null)
                {
                    Console.Out.WriteLine("[+] Bargain nodes: ");
                    foreach (var product_node in bargain_nodes)
                    {
                        IBotMessage message = ExtractProductInfo(product_node);
                        if (message != null)
                        {
                            AddMessage(message);
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

        private bool IsLinkRelevant(HtmlNode listItem)
        {
            return listItem.GetAttributeValue("class", "empty").Equals("empty");
        }
    }
}
