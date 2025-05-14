using ConfigManager;
using ConsoleAppFramework;
using FuzzySharp;
using Models;
using Errors;

namespace Commands;

[RegisterCommands("tag")]
internal class TagCommand
{
    private const int FuzzyRatio = 60;
    private IConfigManager<Configuration> configManager;

    public TagCommand(IConfigManager<Configuration> configManager)
    {
        this.configManager = configManager;
    }


    /// <summary>
    /// List all available tags.
    /// </summary>
    /// <param name="filter">-f, Filter listed tags for tags containing the provided text.</param>
    public async Task List(string? filter = null)
    {
        var (config, error) = await this.configManager.Read();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.Error.WriteLine(error);
            return;
        }

        if (config is null)
        {
            Console.Error.WriteLine(BookmarkErrors.NoConfigurationFound);
            return;
        }

        var tags = config.Bookmarks.SelectMany(b => b.Tags.Select(t => t?.ToLower())).Distinct();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            tags = tags.Where(t => Fuzz.Ratio(t, filter.ToLower()) >= FuzzyRatio);
        }

        string result = tags.Any() ? $"Matchings Tags: {string.Join(',', tags)}" : "No matching tags found";

        Console.WriteLine(result);
    }

    /// <summary>
    /// Search for bookmarks by tag.
    /// </summary>
    public async Task Search([Argument] string tag)
    {
        var (config, error) = await this.configManager.Read();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.Error.WriteLine(error);
            return;
        }

        if (config is null)
        {
            Console.Error.WriteLine(error);
            return;
        }

        config.Bookmarks = config.Bookmarks.Where(b =>
            b.Tags.Any(t => string.Equals(tag, t, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (!config.Bookmarks.Any())
        {
            Console.WriteLine($"No bookmarks found for tag {tag}");
            return;
        }

        Console.WriteLine(config.BookmarkTable());
    }
}

