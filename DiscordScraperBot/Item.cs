using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot
{
    class Item
    {
        private string _name { get; set; }
        private string _link { get; set; }
        private double _price { get; set; }
        private string _contents { get; set; }
        private List<string> _categories { get; set; }
    }
}
