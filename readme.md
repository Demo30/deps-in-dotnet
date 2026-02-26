# Scenario phyb1l - FileLoadException with Inner Exception

## Problem

Binding redirect forces non-existent assembly version → FileLoadException with inner exception.

```
FileLoadException: Could not load 'Assembly, Version=X.X.X.X, PublicKeyToken=...'
---> Inner: Could not load 'Assembly, Version=Y.Y.Y.Y, PublicKeyToken=...'
```

## Structure

```
ServiceConsumer (net472)
└── DirectDependency v1.0.0
    └── TransitiveDependency v1.0.0

App.config: Binding redirect v1 → v2 (doesn't exist)
```

## Root Cause

**App.config**:
```xml
<assemblyBinding>
  <dependentAssembly>
    <assemblyIdentity name="phyb1l_TransitiveDependency" publicKeyToken="245a80169ac37524" />
    <bindingRedirect oldVersion="0.0.0.0-9.9.9.9" newVersion="2.0.0.0" />
  </dependentAssembly>
</assemblyBinding>
```

**Cascade**:
1. DirectDependency compiled against v1.0.0
2. Binding redirect forces v2.0.0
3. v2.0.0 doesn't exist → FileLoadException
4. Fallback to v1.0.0 → also fails (redirect enforced)
5. Inner exception captures fallback attempt

## Testing

```bash
cd ServiceConsumer/ServiceConsumer
dotnet build -c Release
./bin/Release/net472/ServiceConsumer.exe
```

**Expected**:
```
FAILED: FileLoadException
Message: ...Version=2.0.0.0, PublicKeyToken=245a80169ac37524...
Inner Message: ...Version=1.0.0.0, PublicKeyToken=245a80169ac37524...
```

## Real-World Causes

- Misconfigured binding redirects
- Deployment version mismatches
- Package restore pulls wrong version
- CI/CD environment differences

## Key Points

**Strong Naming**: Both versions signed with same key → same PublicKeyToken → enables specific binding redirects.

**Inner Exception**: .NET assembly loading cascade - tries redirect target, then fallback, both fail.

**Comparison to Microsoft Packages**:
```
Real:    Microsoft.Extensions.Configuration.Binder v9.0.0.9 → v9.0.0.7
Our repro: phyb1l_TransitiveDependency v2.0.0.0 → v1.0.0.0
```

Same pattern: Same PublicKeyToken, inner exception shows what was found, binding redirect + version mismatch.
