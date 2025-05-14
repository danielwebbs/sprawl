using System.Text;
using System.Text.Json;
using Models;
using Errors;

namespace ConfigManager;

internal class FileConfigManager : IConfigManager<Configuration>
{
    private string filePath;

    private string SettingsFile => $"{filePath}/sprawl.json";

    private string AliasFile => $"{filePath}/sprawl.sh";

    private string bashrc = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.bashrc";

    ///<summary>
    /// Class <c>FileConfigManager</c> writes config settings to a local file stored on drive for ease of use.
    ///</summary>
    public FileConfigManager(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(ConfigManagerErrors.ValidFilePathRequired);
        }

        this.filePath = filePath;
    }

    ///<summary>
    /// Returns the current configuration if the configuration file exists. 
    /// If no file exists then null is returned.
    ///</summary>
    public async Task<(Configuration?, string? error)> Read()
    {
        if (!File.Exists(SettingsFile))
        {
            return (null, null);
        }

        var data = await File.ReadAllTextAsync(SettingsFile);

        return (JsonSerializer.Deserialize<Configuration>(data, SourceGenerationContext.Default.Configuration), null);
    }

    public async Task<(bool, string? error)> Write(Configuration config)
    {
        if (config == null)
        {
            throw new ArgumentNullException(ConfigManagerErrors.NullConfiguration);
        }

        var data = JsonSerializer.Serialize(config!, SourceGenerationContext.Default.Configuration);

        await File.WriteAllTextAsync(SettingsFile, data);

        var result = await GenerateBookmarkAliases(config);

        return (string.IsNullOrWhiteSpace(result), result);
    }

    private async Task<string?> GenerateBookmarkAliases(Configuration config)
    {
        IEnumerable<string> lines = config.Bookmarks.Select(b => $"alias {b.Name}='cd {b.Path ?? Environment.CurrentDirectory}'");

        if (!File.Exists(this.AliasFile))
        {
            string? error = await this.AddSprawlHooks();

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }
        }

        await File.WriteAllLinesAsync(this.AliasFile, lines);

        return null;
    }

    // Ensure bookmarks aliases apply at start up
    private async Task<string?> AddSprawlHooks()
    {
        //TODO: Add other options
        if (!File.Exists(bashrc))
            return ".bashrc not found";

        var builder = new StringBuilder();
        builder.AppendLine("## Load in sprawl config");
        builder.AppendLine(". ~/.config/sprawl/sprawl.sh");
        await File.AppendAllTextAsync(bashrc, builder.ToString());

        return null;
    }
}
