using System.Text;

namespace Models;

internal class Configuration
{
    public List<BookmarkConfig> Bookmarks { get; set; } = new();

    public string BookmarkTable()
    {
        // Column width for the table produced below.
        // Uses the longest value for each column to determine the max width.
        int[] colWidth = [
            (Bookmarks.Max(b => b.Name?.Length) + 1) ?? 15,
            (Bookmarks.Max(b => b.Path?.Length) + 1) ?? 50,
            (Bookmarks.Max(b => string.Join(',', b.Tags)?.Length) + 1) ?? 50
        ];

        // Build up a table of name and paths for each bookmark.
        // colWidth is used to ensure the coloumns are aligned.
        var builder = new StringBuilder();
        builder.AppendLine(Environment.NewLine);
        builder.AppendLine($"{"Name".PadRight(colWidth[0])} {"Path".PadRight(colWidth[1])} {"Tags".PadRight(colWidth[2])}");
        builder.AppendLine(new string('-', colWidth.Sum()));
        Bookmarks.ForEach(b => builder.AppendLine($"{b.Name?.PadRight(colWidth[0])} {b.Path?.PadRight(colWidth[1])} {string.Join(',', b.Tags)?.PadRight(colWidth[2])}"));

        return builder.ToString();
    }
}
