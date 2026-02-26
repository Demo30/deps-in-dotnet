# Scenario 7h889t - Diamond Dependency in .NET Framework 4.7.2

## Overview

This scenario demonstrates a classic diamond dependency problem in .NET Framework 4.7.2, showing how upgrading a dependency can break existing code at runtime. We have two test scenarios that illustrate the progression from working code to a runtime failure.

## Built Packages (All net472)

### Transitive Dependencies
- **7h889t_TransitiveDependency_A.1.0.0** - ICalculationResult in namespace `TransitiveDependency`
- **7h889t_TransitiveDependency_A.2.0.0** - ICalculationResult moved to `TransitiveDependency.NewNamespace` ⚠️ Breaking Change

### Direct Dependencies
- **7h889t_DirectLibrary_A.1.0.0** - References TransitiveDependency v1.0.0
- **7h889t_DirectLibrary_B.1.0.0** - References TransitiveDependency v1.0.0
- **7h889t_DirectLibrary_B.2.0.0** - References TransitiveDependency v2.0.0

## Scenario 1: Everything Works ✅

**Configuration:**
```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0
│   └── TransitiveDependency v1.0.0
└── DirectDependencyLibraryB v1.0.0
    └── TransitiveDependency v1.0.0
```

**Dependency Resolution:** TransitiveDependency v1.0.0 (both dependencies agree)

**Result:** ✅ Application runs successfully!
```
DirectDepA result: Main app - hello from direct dependency A - Transitive dependency v1.0.0
DirectDepB result: Main app - Hello from Direct dependency B v1.0.0 - Transitive dependency v1.0.0
```

**Why it works:** Both direct dependencies were compiled against and expect TransitiveDependency v1.0.0, and that's exactly what gets loaded at runtime.

---

## Scenario 2: Diamond Dependency Failure 💥

**Configuration:**
```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0
│   └── TransitiveDependency v1.0.0
└── DirectDependencyLibraryB v2.0.0
    └── TransitiveDependency v2.0.0
```

**What Changed:** DirectDependencyLibraryB upgraded from v1.0.0 → v2.0.0, bringing in TransitiveDependency v2.0.0

**Dependency Resolution:** TransitiveDependency v2.0.0 (highest version wins)

**Result:** 💥 Runtime crash with `MissingMethodException`!
```
Unhandled Exception: System.MissingMethodException:
Method not found: 'TransitiveDependency.ICalculationResult
TransitiveDependency.ISomeLogicProcessor.Calculate(System.String)'.
   at DirectDependency.LogicWrapperProcessor.Compute(String input)
```

---

## Why The Crash Happens

### The Breaking Change
TransitiveDependency v2.0.0 introduced a namespace change:
- **v1.0.0**: `ICalculationResult` lives in `TransitiveDependency`
- **v2.0.0**: `ICalculationResult` moved to `TransitiveDependency.NewNamespace`

### The Conflict Timeline

1. **At Compile Time (DirectDepA v1.0.0)**:
   - Compiled against TransitiveDependency v1.0.0
   - Method signature baked into assembly: `TransitiveDependency.ICalculationResult Calculate(string)`

2. **At Runtime**:
   - .NET Framework loads TransitiveDependency v2.0.0 (highest version wins)
   - Actual method signature: `TransitiveDependency.NewNamespace.ICalculationResult Calculate(string)`

3. **Result**:
   - DirectDepA looks for a method returning `TransitiveDependency.ICalculationResult`
   - But only finds a method returning `TransitiveDependency.NewNamespace.ICalculationResult`
   - Type signature mismatch → `MissingMethodException`

---

## The Diamond Dependency Problem

This is called a **diamond dependency** because of the shape:

```
        ServiceConsumer
        /              \
   DirectDepA      DirectDepB v2.0.0
        \              /
         \            /
    TransitiveDep   TransitiveDep
      v1.0.0         v2.0.0
           \         /
            \       /
          Which version?
```

The "diamond" creates a conflict: which version of TransitiveDependency should be used?

---

## Key Takeaways

1. **Upgrading dependencies can break code** - Even if you don't change your code, upgrading a dependency (DirectDepB v1→v2) can introduce breaking changes through its transitive dependencies

2. **"Highest version wins" is dangerous** - .NET Framework's resolution strategy means the highest version of a transitive dependency gets loaded, which may be incompatible with libraries compiled against older versions

3. **Namespace changes are binary breaking changes** - Moving types to different namespaces changes method signatures at the binary level, causing runtime failures

4. **The crash happens at the consumer** - DirectDepA works fine in isolation, but crashes when used in an application where something else forces a newer version of the transitive dependency

5. **This is common in real applications** - Large .NET Framework applications with deep dependency trees often encounter this issue, especially when different teams maintain different libraries

---

## How to Reproduce

### Test Scenario 1 (Working):
```xml
<PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
<PackageReference Include="7h889t_DirectLibrary_B" Version="1.0.0" />
```
Build and run → Works perfectly!

### Test Scenario 2 (Failing):
```xml
<PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
<PackageReference Include="7h889t_DirectLibrary_B" Version="2.0.0" />
```
Build and run → Runtime crash!

---

## Technical Details

- **Framework**: .NET Framework 4.7.2 (net472)
- **Language Version**: C# 7.3 (compatible with net472)
- **Resolution Strategy**: Highest version of transitive dependency wins
- **Failure Type**: Runtime MissingMethodException due to type signature mismatch
- **Breaking Change**: Namespace relocation of return types