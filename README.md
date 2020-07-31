# OzBargain Discord Bot
[![Build status](https://ci.appveyor.com/api/projects/status/7hemkueb44yb9r0v?svg=true)](https://ci.appveyor.com/project/matthewT53/discord-scraper-bot)
## What is this about?
* You can add this Discord bot into your channel and it will scrape the ozBargain website for the latest bargains and discounts.
* Many settings such the period between scraping and the post delay of a bargain can all be adjusted with the commands below.

## Building:
* This project runs on Microsoft C# .Net Core 3 and can be built with Microsoft Visual studio 2019. 
### Packages:
* To obtain the required packages, simply use the restore Nuget packages tool in Visual Studio. 

## Configuring:
### Resources/config.json:
* A configuration file with the relative path "Resources/config.json" to the executable must be created.
* It must have the following contents.
```c#
{
  "token": "Your discord bot token here",
  "commandPrefix": "$",
  "bargainChannelID": "Channel ID that the bot will post to"
}
```
* The Discord bot's token can be found by following [this](https://discordpy.readthedocs.io/en/latest/discord.html).
* To find the channel id, follow this [link](https://stackoverflow.com/questions/41515134/discord-bot-cant-get-channel-by-name).

## Usage
### Commands:
```markdown
# Bot management commands:
help                                Displays the available commands.
info                                Shows some statistics about this discord bot.
show_scrape_delay                   Displays how often web scraping will occur (seconds).
set_scrape_delay <scrapte_rate>     Sets how often the scrapers will run (seconds).
show_depth                          Displays how many links each bot will follow to scrape.
set_depth <bot index> <depth>       Changes how many links the scraper will follow.
show_post_delay                     Displays how often bargains will be posted to the discord channel (milliseconds).
set_post_delay <post delay>         Sets how often bargains will be posted to the discord channel (milliseconds).

# Filtering commands:
add_filter <category>               Adds a category to filter for. e.g electronics
remove_filter <category>            Removes a category from the filter.
show_filters                        Displays all the categories as well as their prices that are being used as filters.

# Filtering commands based on price:
add_filter_price <category> <min price> <max price>         Adds a category and a price to filter for.
remove_filter_price <category> <min price> <max price>      Removes a category and its price from filtering.
update_filter_price <category> <min price> <max price>      Updates an existing filter category with a new price.
```
* The following commands must be prefixed with the commandPrefix.
```
e.g $help
```

## Libraries:
#### 1. Sqlite-net:
* https://github.com/praeclarum/sqlite-net
#### 2. HTTP Agility pack:
* https://html-agility-pack.net/

## Contributing:
* Feel free to fork this repo and create many pull requests for any additional scrapers, bug fixes or new test cases. 

### Coding standards:
* Please read: https://www.dofactory.com/reference/csharp-coding-standards
