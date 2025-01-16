# Motivation for this repo

- Managing project dependencies and having control over them is easy in .NET. Until it's not.
- This project should help us better understand different (problematic) situations that may arise when dealing with dependencies in .NET.

# Content of this repo

- Different scenarios are maintained in separate branches prefixed with "scenario_" followed by an unique short id (to avoid reordering and have persistent reference).
- Each branch contains:
    - published .nupkg packages that are referenced from the main app representing the terminal consumer.
    - NuGet.config with LocalFeed path to these packages
    - readme.md dedicated to each scenario describing what is going on and what problematic behavior we encounter

# Reminder about NuGet's local package cache

- .nupkg packages are prefixed with the scenario's unique id to avoid issues with nuget local package cache on the user's machine
- When experimenting with different versions of packages, beware of the nuget’s local package cache (“.nuget\packages”).
- When the package consumer attempts to restore the package, nuget first attempts to retrieve the package from the cache based on its identification. 
- This means that if we restore package A v1 once and then re-publish the package under the same version with different content (due to local experimentation), the older one will still be used unless the cache is cleaned! 
- Clean up the cache to avoid surprises or use non-conflicting package identifications. 

# Reminder about available syntax for referencing dependencies along with specified version

- Many of the dependency-related issues may occur due to not defining dependency's version strictly enough. Basic \"Version=1.0.0\" means >= 1.0.0, therfore not necessarily version 1.0.0.
- .NET gives us tools to specify versions of our dependencies in more strict/precise way. We may require exact version by: "[1.0.0]" or we can leverage the version ranging syntax "(1.0.0,5.0.0)" etc. See: https://learn.microsoft.com/en-us/nuget/concepts/package-versioning?tabs=semver20sort#version-ranges