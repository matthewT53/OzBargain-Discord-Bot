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

        /***
         * This test ensures that the ozBargain scraper can properly filter out items based on specific 
         * categories. 
         */
        [Fact]
        public void CategoryFilterTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);
            preferences.AddCategory("test_category");

            Scraper scraper = new OzBargainScraper(preferences);

            scraper.AddMessage( 
                new BargainMessage(
                    "test_name", 
                    "test_price", 
                    "test_external_url", 
                    "test_image_url", 
                    "test_category"
                )
            );

            scraper.AddMessage(
                new BargainMessage(
                    "test_name_2",
                    "test_price_2",
                    "test_external_url_2",
                    "test_image_url_2",
                    "test_category_2"
                )
            );

            scraper.AddMessage(
                new BargainMessage(
                    "test_name_3",
                    "test_price_3",
                    "test_external_url_3",
                    "test_image_url_3",
                    "test_category_3 test_category"
                )
            );

            scraper.AddMessage(
                new BargainMessage(
                    "test_name_4",
                    "test_price_4",
                    "test_external_url_4",
                    "test_image_url_4",
                    "test_category_4 &amp test_category_9"
                )
            );

            List<IBotMessage> botMessages = scraper.GetMessages();
            Assert.Equal(2, botMessages.Count);
            Assert.Equal("test_name", botMessages[0].Name);
            Assert.Equal("test_name_3", botMessages[1].Name);
        }
    }
}
