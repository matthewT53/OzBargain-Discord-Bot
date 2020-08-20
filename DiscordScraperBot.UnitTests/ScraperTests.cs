using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DiscordScraperBot.Scrapers;
using DiscordScraperBot.BotMessages;

namespace DiscordScraperBot.UnitTests
{
    public class ScraperTests
    {
        /***
         * This test ensures that duplicate items are not added into the list of items to be posted.
         */
        [Fact]
        public void CacheLayerTest()
        {
            Scraper scraper = new OzBargainScraper(null);

            IBotMessage message = new BargainMessage("test_title", "test_price", "test_image", "test_url", "test_category");
            scraper.AddMessage(message);
            Assert.True(scraper.GetMessages().Count == 1);

            scraper.AddMessage(message);
            Assert.Contains(message, scraper.GetMessages());
            Assert.True(scraper.GetMessages().Count == 1);
        }

        /***
         * This test ensures that the ozBargain scraper can still extract items from the website.
         * If no items are returned then there is a problem.
         */
        [Fact]
        public void OzBargainScrapeTest()
        {
            Scraper scraper = new OzBargainScraper(null);
            scraper.Scrape();
            Assert.True(scraper.GetMessages().Count > 0);
        }
    }
}
