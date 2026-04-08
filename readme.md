# Scenario kx7f2m – Microsoft.Net.Http ↔ System.Net.Http type compatibility

## What is this?

- This is not an app nor a library. This is just educational material on the topic of Dependency resolution in .NET
- The focus is on the conflicts between libraries and related issues when duplicated with differing versions across the dependency tree.

## The scenario

Two direct dependencies expose and consume the same `System.Net.Http` types (`HttpResponseMessage`, `HttpRequestHeaders`) but obtain them through **different NuGet packages**:

| Library | Package used | Version |
|---------|-------------|---------|
| DirectDependencyA v1.0.0 | `Microsoft.Net.Http` 2.2.29 | The **old/deprecated** meta-package |
| DirectDependencyA v2.0.0 | `System.Net.Http` 4.3.4 | The **current** package |
| DirectDependencyB v1.0.0 | `System.Net.Http` 4.3.4 | The **current** package |

### Dependency structure

```
ServiceConsumer (net472)
├── DirectDependencyA v1.0.0 (net472)
│   └── Microsoft.Net.Http 2.2.29
│       ├── Microsoft.Bcl 1.1.10
│       ├── Microsoft.Bcl.Build 1.0.14
│       └── (framework assembly reference: System.Net.Http)
│
└── DirectDependencyB v1.0.0 (net472)
    └── System.Net.Http 4.3.4
```

### What the consumer does

The consumer exercises cross-package type compatibility by:

1. **A → B**: DirectDependencyA creates an `HttpResponseMessage` → consumer passes it to DirectDependencyB for processing
2. **B → A**: DirectDependencyB creates `HttpRequestHeaders` → consumer passes them to DirectDependencyA for reading
3. **Consumer → B**: Consumer itself creates an `HttpResponseMessage` → passes it to DirectDependencyB

## The outcome

### Scenario 1 – DirectDependencyA v1.0.0 (Microsoft.Net.Http) + DirectDependencyB (System.Net.Http)

**✅ All calls succeed.** The types are fully compatible.

On .NET Framework 4.7.2, `Microsoft.Net.Http` does **not** ship its own `System.Net.Http.dll`. For `net45+` targets, it only provides:
- `System.Net.Http.Extensions.dll` (contains `HttpClientHandlerExtensions`)
- `System.Net.Http.Primitives.dll`
- A `<frameworkAssembly>` reference pointing to the built-in `System.Net.Http.dll`

Both libraries compile against the **same** `System.Net.Http, Version=4.2.0.0` (from the SDK reference assemblies targeting pack), and at runtime they load the **same** framework assembly. The types have identical identity.

**Output directory** contains extra DLLs from the `Microsoft.Net.Http` dependency chain:
```
kx7f2m_DirectLibrary_A.dll
kx7f2m_DirectLibrary_B.dll
System.Net.Http.Extensions.dll    ← from Microsoft.Net.Http
System.Net.Http.Primitives.dll    ← from Microsoft.Net.Http
```

### Scenario 2 – DirectDependencyA v2.0.0 (System.Net.Http) + DirectDependencyB (System.Net.Http)

**✅ All calls still succeed.** The migration from `Microsoft.Net.Http` to `System.Net.Http` is fully transparent.

**Output directory** is cleaner — no more legacy Extensions/Primitives DLLs:
```
kx7f2m_DirectLibrary_A.dll
kx7f2m_DirectLibrary_B.dll
```

The transitive dependencies (`Microsoft.Bcl`, `Microsoft.Bcl.Build`) are also gone.

## Key takeaways

1. **Types are identical**: On `net472`, both `Microsoft.Net.Http` and `System.Net.Http` resolve to the same framework `System.Net.Http.dll`. The types (`HttpResponseMessage`, `HttpRequestHeaders`, `HttpClient`, etc.) have the same assembly identity and are fully interchangeable across package boundaries.

2. **`Microsoft.Net.Http` is a meta-package**: For `net45+`, it does not provide its own `System.Net.Http.dll`. It only adds the framework assembly reference plus Extensions/Primitives helper assemblies. For `net40` (where `System.Net.Http` was not part of the framework), it actually shipped the full `System.Net.Http.dll`.

3. **Migration is safe**: Switching from `Microsoft.Net.Http` to `System.Net.Http` on `net472` does not break type compatibility. The only observable change is the removal of transitive dependencies and extra DLLs.

4. **Watch for transitive dependency loss**: If consumer code relies on types from `System.Net.Http.Extensions.dll` (e.g., `HttpClientHandlerExtensions`) that were transitively available through `Microsoft.Net.Http`, upgrading the dependency will remove them. The consumer would need to add its own explicit reference to `Microsoft.Net.Http` to retain those types.

5. **Both compile against the same version**: Despite using different NuGet packages, both libraries compile against `System.Net.Http, Version=4.2.0.0, PublicKeyToken=b03f5f7f11d50a3a` from the SDK reference assemblies. At runtime, the framework provides `System.Net.Http.dll` with assembly version `4.0.0.0`, and the CLR handles the version forwarding transparently.

## How to run

```bash
./test-scenarios.sh
```

Or manually toggle DirectDependencyA version in `ServiceConsumer.csproj`:
```xml
<!-- Scenario 1: Microsoft.Net.Http -->
<PackageReference Include="kx7f2m_DirectLibrary_A" Version="1.0.0" />

<!-- Scenario 2: System.Net.Http -->
<PackageReference Include="kx7f2m_DirectLibrary_A" Version="2.0.0" />
```
