using System.Diagnostics;

namespace Shells;

/// <inheritdoc cref="IShell"/>
public class Bash : IShell
{
    /// <inheritdoc cref="IShell"/>
    public async Task<(string stdout, string stderr)> Execute(string command)
    {
        using var process = this.BuildProcess(command);
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        return (output, error);
    }

    private Process BuildProcess(string command)
    {
        var process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"-c \"{command}\"";
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        return process;
    }
}
