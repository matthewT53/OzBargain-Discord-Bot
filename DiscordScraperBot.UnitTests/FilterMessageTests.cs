using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DiscordScraperBot.UnitTests
{
    public class FilterMessageTests
    {
        /***
         * @description: This test ensures that the bot can correctly filter items.
         * The categories that the item belongs to 
         */
        [Fact]
        public void FilterDesirableItemTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);
            FilterMessage filter = new FilterMessage(preferences);

            BargainMessage message = new BargainMessage(
                "test", "900.50", "external_url", "image_url", 
                "test_category test_category2");

            preferences.AddCategory("test_category2");

            Assert.True(filter.IsDesirable(message));
        }

        /***
         * @description: This test ensures that an undesirable item can be correctly filtered out.
         */
        [Fact]
        public void FilterUndesirableItemTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);
            FilterMessage filter = new FilterMessage(preferences);

            BargainMessage message = new BargainMessage(
                "test", "900.50", "external_url", "image_url",
                "undesirable_category");

            preferences.AddCategory("test_category2");

            Assert.False(filter.IsDesirable(message));
        }

        /***
         * @description: This test ensures that an item can be correctly filtered based on its price. 
         */
        [Fact] 
        public void FilterDesirablePriceTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);
            FilterMessage filter = new FilterMessage(preferences);

            BargainMessage message = new BargainMessage(
                "test", "900.50", "external_url", "image_url",
                "test_category test_category2");

            preferences.AddCategory("test_category");
            preferences.AddPriceRange("test_category", new Tuple<double, double>(100.0, 1000.0));

            Assert.True(filter.IsDesirable(message));
        }

        /***
         * @description: This test ensures that an item with an undesirable price can be filtered out. 
         */
        [Fact] 
        public void FilterUndesirablePriceTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);
            FilterMessage filter = new FilterMessage(preferences);

            BargainMessage message = new BargainMessage(
                "test", "1900.50", "external_url", "image_url",
                "test_category test_category2");

            preferences.AddCategory("test_category");
            preferences.AddPriceRange("test_category", new Tuple<double, double>(100.0, 1000.0));

            Assert.False(filter.IsDesirable(message));
        }
    }
}
