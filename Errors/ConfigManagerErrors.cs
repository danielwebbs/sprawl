namespace Errors;

//Disable xml comment warnings
#pragma warning disable 1591

public static class ConfigManagerErrors
{
    public static string ValidFilePathRequired => "A valid file path must be provided";
    public static string NullConfiguration => "Configuration is null";
}
