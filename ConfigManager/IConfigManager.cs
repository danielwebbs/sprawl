namespace ConfigManager;

///<summary>
/// Manages the reading and updating of sprawl related configuration.
/// </summary>
public interface IConfigManager<T>
{
    ///<summary>
    ///Write configuration object to the sprawl cofiguration file.
    ///</summary>
    public Task<(bool, string? error)> Write(T data);

    ///<summary>
    /// Read the stored sprawl configuration object
    ///</summary>
    public Task<(T?, string? error)> Read();
}
