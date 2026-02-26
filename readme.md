# Scenario 7h889t - Diamond Dependency in .NET Framework 4.7.2

## The Problem

Diamond dependency: when your app uses libraries A and B, which both depend on different versions of library C. .NET Framework's "highest version wins" strategy can cause runtime crashes.

## Structure

```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0 → TransitiveDependency v1.0.0
└── DirectDependencyLibraryB v1.0.0 → TransitiveDependency v1.0.0  ✅ Works
    DirectDependencyLibraryB v2.0.0 → TransitiveDependency v2.0.0  💥 Partial Crash
```

**Breaking change in TransitiveDep v2.0.0:** `ICalculationResult` moved from `TransitiveDependency` namespace to `TransitiveDependency.NewNamespace`

## Method-Specific Failure

DirectDepA has two methods:
- `ComputeSimple()` - returns `string` → **Works even with v2.0.0**
- `Compute()` - returns `ICalculationResult` → **Crashes with v2.0.0**

The crash is **method-specific**, not library-wide. Only methods with incompatible signatures fail.

## Testing

Edit `ServiceConsumer/ServiceConsumer/ServiceConsumer.csproj`:

**Scenario 1 (all methods work):**
```xml
<PackageReference Include="7h889t_DirectLibrary_B" Version="1.0.0" />
```
Output:
```
ComputeSimple: ... - Simple calculation v1.0.0
Compute: ... - Transitive dependency v1.0.0
```

**Scenario 2 (partial failure):**
```xml
<PackageReference Include="7h889t_DirectLibrary_B" Version="2.0.0" />
```
Output:
```
ComputeSimple: ... - Simple calculation v2.0.0  ✅ Works!
Compute FAILED: MissingMethodException  💥 Crashes!
```

Build and run:
```bash
cd ServiceConsumer
dotnet build -c Release
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
```

## Why Method-Specific

1. `ComputeSimple()` has signature: `string CalculateSimple(string)` - no namespace-dependent types
2. `Compute()` has signature: `TransitiveDependency.ICalculationResult Calculate(string)` - depends on type that moved
3. Runtime can find `CalculateSimple` in v2.0.0 (signature unchanged)
4. Runtime cannot find `Calculate` with `TransitiveDependency.ICalculationResult` return type (now in `NewNamespace`)

## Key Points

- Crashes are **per-method**, not per-library
- Only methods using types that changed signature will fail
- Methods using primitives or unchanged types continue to work
- This makes debugging harder - some functionality works, some doesn't