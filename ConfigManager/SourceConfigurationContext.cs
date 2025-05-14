using System.Text.Json.Serialization;
using Models;

namespace ConfigManager;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Configuration))]
[JsonSerializable(typeof(BookmarkConfig))]
[JsonSerializable(typeof(IEnumerable<BookmarkConfig>))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

// Due to this application using AOT compliation is it required that we create the Source Generation Context ourselves.
// Any new classes which require serlisation need to be added to the above.
// Read more at https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation
