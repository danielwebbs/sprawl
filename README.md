# Sprawl

**Sprawl** is a lightweight .NET console application designed to make fast navigation between directories easy through the use of bookmarks.

I mainly made this as a fun way to solve a problem I had as well as test out dotnets Native AOT (which is really cool!)

Currently Sprawl only supports Linux and macOS and expects a `.bashrc` to be present. I currently have no plans to support Windows as I don't use it.

## Features

- Manage bookmarks to navigate to directories quickly
- Tags provide an easy way to group bookmarks
- Forgot what bookmarks you have? Just list them!

## Requirements

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)

## Installation

Clone the repository and publish the application
```
dotnet publish -o release
```

To add a bookmark simply run 

```
sprawl bookmark add {name}
```

You will need to reload your bashrc after a bookmark is added

```
source ~/.bashrc
```

You can now instantly navigate to the bookmarks directory by simply using the bookmarks name!

```
{name}
```
