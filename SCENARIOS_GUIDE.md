# Diamond Dependency Scenarios - Complete Guide

## 🎯 What Was Built

All projects have been transformed to **NET Framework 4.7.2** and built as NuGet packages to demonstrate the diamond dependency problem.

### 📦 Package Inventory

#### Transitive Dependencies
| Package | Version | Key Feature |
|---------|---------|-------------|
| 7h889t_TransitiveDependency_A | 1.0.0 | `ICalculationResult` in `TransitiveDependency` namespace |
| 7h889t_TransitiveDependency_A | 2.0.0 | `ICalculationResult` in `TransitiveDependency.NewNamespace` ⚠️ |

#### Direct Dependencies
| Package | Version | References |
|---------|---------|------------|
| 7h889t_DirectLibrary_A | 1.0.0 | TransitiveDependency v1.0.0 |
| 7h889t_DirectLibrary_B | 1.0.0 | TransitiveDependency v1.0.0 |
| 7h889t_DirectLibrary_B | 2.0.0 | TransitiveDependency v2.0.0 |

---

## 🧪 Test Scenarios

### Scenario 1: Harmonious Dependencies ✅

**ServiceConsumer Configuration:**
```xml
<PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
<PackageReference Include="7h889t_DirectLibrary_B" Version="1.0.0" />
```

**Dependency Graph:**
```
ServiceConsumer
├── DirectLibraryA v1.0.0 → TransitiveDependency v1.0.0
└── DirectLibraryB v1.0.0 → TransitiveDependency v1.0.0

Resolved: TransitiveDependency v1.0.0 ✅
```

**Expected Behavior:** Application runs successfully!

**Output:**
```
DirectDepA result: Main app - hello from direct dependency A - Transitive dependency v1.0.0
DirectDepB result: Main app - Hello from Direct dependency B v1.0.0 - Transitive dependency v1.0.0
```

**Why it works:**
- Both DirectDepA and DirectDepB were compiled against TransitiveDependency v1.0.0
- At runtime, TransitiveDependency v1.0.0 is loaded
- Method signatures match expectations → No issues!

---

### Scenario 2: Diamond Dependency Conflict 💥

**ServiceConsumer Configuration:**
```xml
<PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
<PackageReference Include="7h889t_DirectLibrary_B" Version="2.0.0" />
```

**Dependency Graph:**
```
ServiceConsumer
├── DirectLibraryA v1.0.0 → TransitiveDependency v1.0.0
└── DirectLibraryB v2.0.0 → TransitiveDependency v2.0.0

Resolved: TransitiveDependency v2.0.0 ⚠️ (highest version wins)
```

**Expected Behavior:** Runtime crash with `MissingMethodException`!

**Error:**
```
Unhandled Exception: System.MissingMethodException:
Method not found: 'TransitiveDependency.ICalculationResult
TransitiveDependency.ISomeLogicProcessor.Calculate(System.String)'.
   at DirectDependency.LogicWrapperProcessor.Compute(String input)
   at Program.Main(String[] args)
```

**Why it crashes:**
- DirectDepA was compiled expecting `TransitiveDependency.ICalculationResult`
- At runtime, TransitiveDependency v2.0.0 is loaded (highest version wins)
- v2.0.0 has `TransitiveDependency.NewNamespace.ICalculationResult`
- Method signature mismatch → `MissingMethodException`!

---

## 🔍 The Root Cause

### The Breaking Change

TransitiveDependency v2.0.0 introduced a **namespace migration**:

**v1.0.0:**
```csharp
namespace TransitiveDependency
{
    public interface ICalculationResult { string Result { get; } }
}
```

**v2.0.0:**
```csharp
namespace TransitiveDependency.NewNamespace  // ⚠️ MOVED!
{
    public interface ICalculationResult { string Result { get; } }
}
```

This changes the **fully qualified type name**, which is part of the method signature at the binary level.

### What Happens at Runtime

1. **DirectDepA v1.0.0 expects:** `TransitiveDependency.ICalculationResult Calculate(string)`
2. **TransitiveDependency v2.0.0 provides:** `TransitiveDependency.NewNamespace.ICalculationResult Calculate(string)`
3. **CLR says:** "These are different methods!" → `MissingMethodException`

---

## 📊 Comparison Table

| Aspect | Scenario 1 (Working) | Scenario 2 (Failing) |
|--------|---------------------|----------------------|
| **DirectDepA** | v1.0.0 → TransDep v1.0.0 | v1.0.0 → TransDep v1.0.0 |
| **DirectDepB** | v1.0.0 → TransDep v1.0.0 | v2.0.0 → TransDep v2.0.0 |
| **Resolved Version** | TransDep v1.0.0 | TransDep v2.0.0 |
| **DirectDepA Happy?** | ✅ Yes (gets v1.0.0) | ❌ No (gets v2.0.0) |
| **DirectDepB Happy?** | ✅ Yes (gets v1.0.0) | ✅ Yes (gets v2.0.0) |
| **Result** | Both work! | DirectDepA crashes! |

---

## 🎓 Learning Points

### 1. Version Upgrades Can Break Code
Even without changing your own code, upgrading DirectDepB from v1.0.0 to v2.0.0 introduces a breaking change that affects DirectDepA.

### 2. Highest Version Wins (But May Lose)
.NET Framework's "highest version wins" strategy for transitive dependencies can force libraries to run with versions they weren't designed for.

### 3. Namespace Changes Are Binary Breaking
Moving types to different namespaces changes their identity at the binary level, making old compiled code incompatible.

### 4. The Diamond Problem
```
        Consumer
        /      \
    DepA      DepB v2
        \      /
      TransDep v1  vs  TransDep v2
           \         /
          Which one?
```
When two paths request different versions, only one can win—and the other loses.

### 5. Failure Happens at the Consumer
DirectDepA works perfectly in isolation. It only fails when used in an environment where something else (DirectDepB v2.0.0) forces a newer version of the transitive dependency.

---

## 🚀 How to Test

### Build Everything
```bash
# Already built! All packages are in _BuiltPackages/
ls _BuiltPackages/
```

### Test Scenario 1 (Working)
1. Edit `ServiceConsumer/ServiceConsumer/ServiceConsumer.csproj`
2. Set:
   ```xml
   <PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
   <PackageReference Include="7h889t_DirectLibrary_B" Version="1.0.0" />
   ```
3. Build and run:
   ```bash
   cd ServiceConsumer
   dotnet build -c Release
   ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
   ```
4. Expected: Success! ✅

### Test Scenario 2 (Failing)
1. Edit `ServiceConsumer/ServiceConsumer/ServiceConsumer.csproj`
2. Set:
   ```xml
   <PackageReference Include="7h889t_DirectLibrary_A" Version="1.0.0" />
   <PackageReference Include="7h889t_DirectLibrary_B" Version="2.0.0" />
   ```
3. Build and run:
   ```bash
   cd ServiceConsumer
   dotnet build -c Release
   ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
   ```
4. Expected: Runtime crash! 💥

---

## 🎯 Real-World Implications

This scenario represents a common problem in large .NET Framework applications:

1. **Library A** depends on Newtonsoft.Json 10.0.0
2. **Library B** upgrades to Newtonsoft.Json 13.0.0
3. Your app uses both A and B
4. Newtonsoft.Json 13.0.0 is loaded (highest version wins)
5. Library A crashes because it expects 10.0.0's API

**Solutions:**
- Binding redirects (app.config)
- Keep all dependencies synchronized
- Use assembly versioning strategically
- Migrate to .NET Core/.NET 5+ with better isolation

---

## 📝 Summary

This repository demonstrates that:
- ✅ All projects converted to .NET Framework 4.7.2
- ✅ 5 NuGet packages built successfully
- ✅ Diamond dependency created and working
- ✅ Scenario 1 shows harmonious dependencies (works)
- ✅ Scenario 2 shows diamond conflict (crashes)
- ✅ README.md updated with comprehensive documentation

The diamond dependency is real, reproducible, and educational! 🎓
