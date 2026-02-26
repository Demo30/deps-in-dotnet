# Scenario 7h889t - Diamond Dependency in .NET Framework 4.7.2

## The Problem

Diamond dependency: when your app uses libraries A and B, which both depend on different versions of library C. .NET Framework's "highest version wins" strategy can cause runtime crashes.

## Structure

```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0 → TransitiveDependency v1.0.0
└── DirectDependencyLibraryB v1.0.0 → TransitiveDependency v1.0.0  ✅ Works
    DirectDependencyLibraryB v2.0.0 → TransitiveDependency v2.0.0  💥 Crashes
```

**Breaking change in TransitiveDep v2.0.0:** `ICalculationResult` moved from `TransitiveDependency` namespace to `TransitiveDependency.NewNamespace`

## Testing

Edit `ServiceConsumer/ServiceConsumer/ServiceConsumer.csproj`:

**Scenario 1 (works):**
```xml
<PackageReference Include="7h889t_DirectLibrary_B" Version="1.0.0" />
```

**Scenario 2 (crashes):**
```xml
<PackageReference Include="7h889t_DirectLibrary_B" Version="2.0.0" />
```

Build and run:
```bash
cd ServiceConsumer
dotnet build -c Release
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
```

## Why It Crashes

1. DirectDepA compiled against TransitiveDep v1.0.0 (expects `TransitiveDependency.ICalculationResult`)
2. Runtime loads TransitiveDep v2.0.0 (provides `TransitiveDependency.NewNamespace.ICalculationResult`)
3. Method signature mismatch → `MissingMethodException`

## Key Points

- Upgrading one dependency can break unrelated code
- Namespace changes are binary breaking changes
- "Highest version wins" forces incompatible versions on older libraries
- Common in large .NET Framework apps with deep dependency trees