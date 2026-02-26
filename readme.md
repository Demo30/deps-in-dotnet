# Scenario 7h889t - Diamond Dependency in .NET Framework 4.7.2

## Overview

This scenario demonstrates a classic diamond dependency problem in .NET Framework 4.7.2, where two direct dependencies require different versions of the same transitive dependency, leading to runtime failures.

## Reference Tree

```
ServiceConsumer (Main app - net472)
├── DirectDependencyLibraryA v1.0.0 (net472)
│   └── TransitiveDependency v1.0.0 (net472)
│       └── ICalculationResult in namespace: TransitiveDependency
└── DirectDependencyLibraryB v2.0.0 (net472)
    └── TransitiveDependency v2.0.0 (net472)
        └── ICalculationResult in namespace: TransitiveDependency.NewNamespace
```

## Diamond Dependency Configuration

- **TransitiveDependency v1.0.0**:
  - Return type `ICalculationResult` is in namespace `TransitiveDependency`
  - Message: "Transitive dependency v1.0.0"

- **TransitiveDependency v2.0.0**:
  - **BREAKING CHANGE**: Return type `ICalculationResult` moved to namespace `TransitiveDependency.NewNamespace`
  - Message: "Transitive dependency v2.0.0"

- **DirectDependencyLibraryA v1.0.0**:
  - References TransitiveDependency v1.0.0
  - Compiled against `TransitiveDependency.ICalculationResult`

- **DirectDependencyLibraryB v2.0.0**:
  - References TransitiveDependency v2.0.0
  - Compiled against `TransitiveDependency.NewNamespace.ICalculationResult`

## Actual Dependency Resolution

.NET Framework resolves to **TransitiveDependency v2.0.0** (highest version wins), which means:
- Both DirectDependencyLibraryA and DirectDependencyLibraryB will use TransitiveDependency v2.0.0 at runtime
- DirectDependencyLibraryA was compiled expecting v1.0.0's interface signatures

## Runtime Behavior

**Error**: `System.MissingMethodException: Method not found: 'TransitiveDependency.ICalculationResult TransitiveDependency.ISomeLogicProcessor.Calculate(System.String)'`

### Why This Happens

1. DirectDependencyLibraryA was compiled against TransitiveDependency v1.0.0
2. At compile time, it expects `ISomeLogicProcessor.Calculate()` to return `TransitiveDependency.ICalculationResult`
3. At runtime, .NET loads TransitiveDependency v2.0.0 (due to version resolution)
4. TransitiveDependency v2.0.0's `Calculate()` method returns `TransitiveDependency.NewNamespace.ICalculationResult`
5. The type signature mismatch causes a `MissingMethodException`

## Key Takeaways

- **Diamond dependencies** occur when multiple paths lead to different versions of the same package
- .NET Framework uses **highest version wins** strategy for dependency resolution
- **Namespace changes** in dependencies are binary breaking changes
- Libraries compiled against one version may fail at runtime when a different version is loaded
- This is a common issue in large .NET Framework applications with complex dependency trees

## Technical Details

- **Framework**: .NET Framework 4.7.2 (net472)
- **Language Version**: C# 7.3 (compatible with net472)
- **Resolution Strategy**: Highest version of transitive dependency wins
- **Failure Type**: Runtime MissingMethodException due to type signature mismatch