using ConfigManager;
using ConsoleAppFramework;
using Models;
using Errors;
using Shells;
using Validators;

namespace Commands;

[RegisterCommands("bookmark")]
internal class Bookmark
{
    private IConfigManager<Configuration> configManager;
    private BookmarkCommandValidator validator;

    public Bookmark(IConfigManager<Configuration> configManager, BookmarkCommandValidator validator)
    {
        this.configManager = configManager;
        this.validator = validator;
    }

    /// <summary>
    /// Add a new bookmark. If a path is not provided the current working directory will be used.
    /// </summary>
    /// <param name="name"> bookmark name. This will be the name used to navigate to a given directory using this bookmark. </param>
    /// <param name="tags">-t, tags to be associated with the bookmark.</param>
    /// <param name="path">-p, the path to link the bookmark to. </param>
    public async Task Add([Argument] string name,
                          string? path = null,
                          string[]? tags = null)
    {

        var (config, error) = await this.configManager.Read();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.Error.WriteLine(error ?? BookmarkErrors.FailedToLoadConfig);
            return;
        }

        if (config is null)
            config = new Configuration();

        var (result, verror) = await this.validator.Validate(config, name);

        if (!result)
        {
            Console.Error.WriteLine(verror ?? "Invalid Bookmark");
            return;
        }

        config.Bookmarks.Add(new BookmarkConfig
        {
            Name = name,
            Path = path ?? Environment.CurrentDirectory,
            Tags = tags?.ToList() ?? new List<string>()
        });

        var (success, wError) = await configManager.Write(config);

        if (!success)
        {
            Console.Error.WriteLine(wError ?? BookmarkErrors.FailedToAddBookmark);
        }

        Console.WriteLine($"{name} added. Please source your bashrc to refresh bookmarks in the current shell.");
    }

    /// <summary>
    /// Delete a bookmark by name.
    /// </summary>
    public async Task Delete([Argument] string bookmarkName)
    {
        var (config, error) = await this.configManager.Read();

        if (config is null)
        {
            Console.Error.WriteLine(BookmarkErrors.NoConfigurationFound);
            return;
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.Error.WriteLine(error);
            return;
        }

        var bookmarks = config.Bookmarks.Where(b => !string.Equals(bookmarkName, b.Name, StringComparison.OrdinalIgnoreCase)).ToList();
        config.Bookmarks = bookmarks;

        var (result, wError) = await this.configManager.Write(config);

        if (!result)
            Console.Error.WriteLine(result);

        Console.WriteLine($"Deleting {bookmarkName}");
    }

    /// <summary>
    /// List all bookmarks. Includes additional data such as tags 
    /// </summary>
    public async Task List()
    {
        var (config, error) = await this.configManager.Read();

        if (config is null)
        {
            Console.Error.WriteLine(BookmarkErrors.NoConfigurationFound);
            return;
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.Error.WriteLine(error);
            return;
        }

        Console.WriteLine(config.BookmarkTable());
    }
}
