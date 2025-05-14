namespace Errors;

//Disable xml comment warnings
#pragma warning disable 1591

public static class BookmarkErrors
{
    public static string FailedToLoadConfig => "Failed to load configuration";
    public static string FailedToAddBookmark => "Failed to add bookmark";
    public static string BookmarkNameUnavailable => "A different application already uses this bookmark name";
    public static string BookmarkNameInUse => "Bookmark name already in use";
    public static string NoConfigurationFound => "No configuration found";
}
