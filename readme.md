# Scenario phyb1l - Missing Transitive Dependency (FileNotFoundException)

## The Problem

DirectLibrary uses MyLibrary but marks it with `PrivateAssets="All"`, so MyLibrary is NOT declared as a transitive dependency. ServiceConsumer only gets DirectLibrary during restore, causing runtime FileNotFoundException.

## Structure

```
ServiceConsumer (net472)
└── DirectLibrary v1.0.0
    └── MyLibrary v1.0.0 (PrivateAssets="All") ← NOT restored!
```

## Root Cause

DirectLibrary.csproj:
```xml
<PackageReference Include="phyb1l_MyLibrary" Version="1.0.0" PrivateAssets="All" />
```

`PrivateAssets="All"` means:
- DirectLibrary can use MyLibrary at compile time
- MyLibrary is NOT listed in DirectLibrary's package dependencies
- Consumers of DirectLibrary won't get MyLibrary

## Testing

Normal workflow fails:
```bash
cd ServiceConsumer
dotnet restore    # ✅ Succeeds - only gets DirectLibrary
dotnet build      # ✅ Succeeds - compiles fine
dotnet run        # 💥 FileNotFoundException!
```

Check bin folder:
```bash
ls ServiceConsumer/bin/Release/net472/*.dll
# Only shows: phyb1l_DirectLibrary.dll
# Missing: phyb1l_MyLibrary.dll
```

## Error

```
FAILED: FileNotFoundException
Message: Could not load file or assembly 'phyb1l_MyLibrary, Version=1.0.0.0,
Culture=neutral, PublicKeyToken=null' or one of its dependencies.
```

## Real-World Causes

- `PrivateAssets="All"` on dependencies that should be transitive
- Incorrect package metadata (missing dependency declarations)
- Dependencies marked as development-only accidentally
- Hand-edited .nuspec files with missing `<dependencies>` entries