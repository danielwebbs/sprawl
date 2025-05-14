using Errors;
using Models;
using Shells;

namespace Validators;

///<summary>
/// Responsible for validating that a new bookmark is safe to create
///</summary>
internal class BookmarkCommandValidator(IShell shell)
{
    public async Task<(bool result, string? error)> Validate(Configuration config, string bookmarkName)
    {
        if (config.Bookmarks.Any(b => b?.Name?.Equals(bookmarkName, StringComparison.OrdinalIgnoreCase) == true))
        {
            return (false, BookmarkErrors.BookmarkNameInUse);
        }

        // Check if the provided bookmark name is safe to use
        (string stdOut, string stdErr) = await shell.Execute($"type {bookmarkName}");
        if (!string.IsNullOrWhiteSpace(stdOut) || (string.IsNullOrWhiteSpace(stdErr)) || !stdErr.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return (false, BookmarkErrors.BookmarkNameUnavailable);
        }

        return (true, null);
    }
}
