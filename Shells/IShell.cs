namespace Shells;

/// <summary> 
/// Manages the creation of a process in a given shell.
/// The process is only created when executing a command and is killed shortly after.
/// </summary>
public interface IShell
{
    /// <summary>
    /// Execute a command in a shell environment
    /// </summary>
    /// <param name="command">Command to be executed </param>
    public Task<(string stdout, string stderr)> Execute(string command);
}
