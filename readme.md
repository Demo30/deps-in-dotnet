# What is this?

- This is not an app nor a library. This is just educational material on the topic of Dependency resolution in .NET
- The focus is on the conflicts between libraries and related issues when duplicated with differing versions across the dependency tree.

# Motivation for this repo

- Managing project dependencies and having control over them is easy in .NET. Until it's not.
- This project should help us better understand different (problematic) situations that may arise when dealing with dependencies in .NET.

# Content of this repo

- Different scenarios are maintained in separate branches prefixed with "scenario/" followed by an unique short id (to avoid reordering and have persistent reference).
- Each branch contains:
    - published .nupkg packages that are referenced from the main app representing the terminal consumer.
    - NuGet.config with LocalFeed path to these packages
    - readme.md dedicated to each scenario describing what is going on and what problematic behavior we encounter

## Scenario c61417, 25a851

- Transitive dependency is referenced from the main project directly with lower version, but higher version of this Transitive dependency is also used by another referenced package.

## Scenario a0a93f, ea0545, mha17d

- Transitive dependency is referenced from the main project directly with higher version, but lower version of this Transitive dependency is also used by Direct dependency package reference by the Main app.

- a0a93f: lower version reference in Direct dependency is strict ("[1.0.0]")

- ea0545: lower version reference in Direct dependency is non-strict ("1.0.0")

- mha17d: lower version reference in Direct dependency is non-strict ("1.0.0") and also the higher version of Transitive dependency has changed return type of used method.

## Scenario a4328d, c2fc88, d0c14d, 05d335

- There are two different packages referenced which both in turn reference the same Transitive dependency. One of these packages uses lower version. Another one uses a higher version.

- c2fc88: further illustrates how the Transitive dependency may be outside of our focus (since we don't use functionality of Direct dependency B that uses it.).

- d0c14d: focuses on introduced nullability from the Transitive dependency and its consequences to the Direct dependency A and its method signature.

- 05d335: Direct dependency B forces usage of higher Transitive dependency in Direct dependency A. Transitive dependency has changed namespace of its return type.

## Scenario ac8156

- The main app used to reference “Direct dependency” by “1.0.0” which in turn referenced “Transitive dependency” by “[1.0.0]”. The main app uses the Transitive dependency directly without explicitly referencing it.

## Scenario bc9686

- Another level of dependency tree is introduced along with both A) flow drilling all the way down to the second level transitive dependency that **is still working fine** and B) flow drilling all the way down to the second level transitive dependency that **crashes in runtime call**.

## .NET Framework scenarios

The scenarios below are specific to .NET Framework (4.7.2) and involve concepts such as assembly binding redirects and strong naming that do not apply to .NET (Core / 5+).

### Scenario phyb1l

- A misconfigured binding redirect in `App.config` redirects all versions (0.0.0.0–9.9.9.9) of the Transitive dependency to version 2.0.0.0, which does not exist. The runtime cannot load the assembly and throws a `FileLoadException`. The inner exception documents a failed fallback attempt back to version 1.0.0.0, which is also blocked by the redirect — resulting in a cascading nested exception failure.

### Scenario 7h889t

- A diamond dependency problem where both Direct dependency A and Direct dependency B reference the same Transitive dependency at different versions. Due to the "highest version wins" resolution strategy, a breaking change in the Transitive dependency (a namespace relocation of `ICalculationResult`) causes a **partial runtime failure**: methods returning primitive types (`ComputeSimple()`) continue to work, while methods depending on the relocated type (`Compute()`) throw a `MissingMethodException`.

### Scenario vp1143

- The same diamond dependency structure as `7h889t`, but Transitive dependency v2.0.0 is **backwards compatible** (no breaking changes). Both direct dependencies are consumed as pre-built packages — simulating third-party vendor libraries whose source you don't own. The consumer resolves the conflict in two steps: an explicit NuGet version pin (to satisfy the package graph) and a binding redirect in `App.config` (to tell the CLR to serve v2.0.0 to anyone asking for v1.0.0 at runtime). Contrasts `7h889t` by showing when a binding redirect **can** fix the diamond problem.

# Reminder about NuGet's local package cache

- .nupkg packages are prefixed with the scenario's unique id to avoid issues with nuget local package cache on the user's machine
- When experimenting with different versions of packages, beware of the nuget’s local package cache (“.nuget\packages”).
- When the package consumer attempts to restore the package, nuget first attempts to retrieve the package from the cache based on its identification. 
- This means that if we restore package A v1 once and then re-publish the package under the same version with different content (due to local experimentation), the older one will still be used unless the cache is cleaned! 
- Clean up the cache to avoid surprises or use non-conflicting package identifications. 

# Reminder about available syntax for referencing dependencies along with specified version

- Many of the dependency-related issues may occur due to not defining dependency's version strictly enough. Basic \"Version=1.0.0\" means >= 1.0.0, therfore not necessarily version 1.0.0.
- .NET gives us tools to specify versions of our dependencies in more strict/precise way. We may require exact version by: "[1.0.0]" or we can leverage the version ranging syntax "(1.0.0,5.0.0)" etc. See: https://learn.microsoft.com/en-us/nuget/concepts/package-versioning?tabs=semver20sort#version-ranges
