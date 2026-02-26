# Scenario phyb1l - Transitive Dependency Missing (FileNotFoundException)

## The Problem

Application uses DirectLibrary which depends on MyLibrary v1.0.0 (transitive dependency). If MyLibrary DLL is missing at runtime, FileNotFoundException occurs.

## Structure

```
ServiceConsumer (net472)
└── DirectLibrary v1.0.0
    └── MyLibrary v1.0.0 (transitive) ← Missing!
```

## Scenario

1. ServiceConsumer references DirectLibrary
2. DirectLibrary depends on MyLibrary v1.0.0 (transitive)
3. MyLibrary DLL missing from bin folder
4. Runtime tries to load MyLibrary → FileNotFoundException

## Error

```
FAILED: FileNotFoundException
Message: Could not load file or assembly 'phyb1l_MyLibrary, Version=1.0.0.0,
Culture=neutral, PublicKeyToken=null' or one of its dependencies.
The system cannot find the file specified.
```

## Testing

Build and test:
```bash
cd ServiceConsumer
dotnet build -c Release

# Works (MyLibrary present):
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe

# Fails (MyLibrary removed):
rm ServiceConsumer/bin/Release/net472/phyb1l_MyLibrary.dll
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
```

## Key Points

- Transitive dependencies must be present at runtime
- Missing transitive DLL triggers FileNotFoundException
- Error occurs on first use of type from direct dependency
- Common when DLLs manually deleted or deployment incomplete