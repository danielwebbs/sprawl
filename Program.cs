using ConfigManager;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Shells;
using Validators;

// Create the directory for all sprawl files to be stored in if it does not exist
string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/sprawl";

if (!Directory.Exists(path))
{
    _ = Directory.CreateDirectory(path);

    Console.WriteLine($"Sprawl is running for the first time. A config directory has been created in {path}");
}

Console.WriteLine(path);

var app = ConsoleAppFramework.ConsoleApp.Create()
.ConfigureServices(service =>
    {
        service.AddTransient<IShell, Bash>();
        service.AddTransient<BookmarkCommandValidator, BookmarkCommandValidator>();
        service.AddTransient<IConfigManager<Configuration>>(x =>
        {
            var shell = x.GetService<IShell>();

            if (shell is null)
                throw new ArgumentNullException(nameof(IShell));

            return new FileConfigManager(path);
        });
    });
app.Run(args);
