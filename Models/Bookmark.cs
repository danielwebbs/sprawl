namespace Models;

internal class BookmarkConfig
{
    public string? Name { get; set; }

    public string? Path { get; set; }

    public List<string> Tags { get; set; } = new();
}

