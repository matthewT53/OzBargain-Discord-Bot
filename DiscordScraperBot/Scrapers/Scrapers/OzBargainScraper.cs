using System;
using System.Collections.Generic;
using System.Linq;
using DiscordScraperBot.BotMessages;
using HtmlAgilityPack;
using LLibrary;

namespace DiscordScraperBot.Scrapers
{
    public class OzBargainScraper : Scraper
    {
        const string BaseUrl = "https://www.ozbargain.com.au";

        const string BargainsXpath = "//*[@class=\"node node-ozbdeal node-teaser\"]";
        const string BargainTitleXPath = ".//h2//a//text()";
        const string BargainExternalLinkXPath = ".//div[2]//div[1]//div//a";
        const string ListOfNextPagesXPath = "//*[@id=\"main\"]/ul";
        const string CategoryXPath = ".//div[4]/ul/li[2]/span/a";

        const string ScraperName = "OzBargainScraper";

        public OzBargainScraper(Preferences preferences)
            : base(preferences, ScraperName)
        { 
        }

        public override void Scrape()
        {
            HtmlWeb web = new HtmlWeb();

            var baseHtml = web.Load(BaseUrl);
            HashSet<string> linksToFollow = ExtractNextLinks(baseHtml);
            linksToFollow.Add(BaseUrl);

            int currentDepth = 1;
            foreach (string link in linksToFollow)
            {
                var htmlDoc = web.Load(link);

                var bargainNodes = htmlDoc.DocumentNode.SelectNodes(BargainsXpath);
                if (bargainNodes != null)
                {
                    foreach (var productNode in bargainNodes)
                    {
                        IBotMessage message = ExtractProductInfo(productNode);
                        if (message != null)
                        {
                            AddMessage(message);
                        }
                    }
                }

                if (currentDepth >= Depth)
                {
                    break;
                }

                currentDepth++;
            }
        }

        public override void Filter()
        {
            List<UserPreference> preferences = UserPreferences.UserPreferences;
            List<IBotMessage> filteredMessages = new List<IBotMessage>();

            if (preferences.Count != 0)
            {
                foreach (BargainMessage message in GetMessages())
                {
                    foreach (UserPreference preference in preferences)
                    {
                        if (message.Categories.Contains(preference._category))
                        {
                            if (preference._minPrice > 0.0 && preference._maxPrice > 0.0)
                            {
                                double messagePrice = Convert.ToDouble(message.Price);
                                if (messagePrice >= preference._minPrice && messagePrice <= preference._maxPrice)
                                {
                                    filteredMessages.Add(message);
                                }
                            } 

                            else
                            {
                                filteredMessages.Add(message);
                            }
                            
                            break;
                        }
                    }
                }

                base.Messages = filteredMessages;
            }
        }

        /***
         * This method extracts information about a product found on the OzBargain website.
         * Filtering of objects is also done here.
         * Input:
         *  - HtmlNode object.
         * Returns a BargainMessage object with the extracted inforamtion. 
         */
        private BargainMessage ExtractProductInfo(HtmlNode productNode)
        {
            string name = "";
            string price = "";
            string externalUrl = "";
            string imageUrl = "";
            string category = "";

            try
            {
                var titleNodes = productNode.SelectNodes(BargainTitleXPath);
                if (titleNodes == null || titleNodes.Count == 0)
                {
                    return null;
                }

                name = titleNodes.Count >= 1 ? titleNodes[0].InnerText : "";
                price = titleNodes.Count >= 2 ? titleNodes[1].InnerText : "";

                var rightNodes = productNode.SelectNodes(BargainExternalLinkXPath);
                if (rightNodes == null || rightNodes.Count == 0)
                {
                    return null;
                }

                externalUrl = BaseUrl + rightNodes[0].Attributes["href"].Value;
                imageUrl = rightNodes[0].FirstChild.Attributes["src"].Value;

                var categoryNode = productNode.SelectSingleNode(CategoryXPath);
                if (categoryNode == null)
                {
                    return null;
                }

                category = categoryNode.InnerText.ToLower();
            }

            catch (Exception e)
            {
                Logger logger = Logger.GetInstance();
                logger.realLogger.Error(e);
            }

            BargainMessage message = new BargainMessage(name, price, externalUrl, imageUrl, category);
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
