# Scenario vp1143 - Binding Redirect as the Correct Solution

## Summary

This scenario demonstrates a **good use of binding redirects** to resolve a diamond dependency problem.
Unlike scenario `7h889t` where the conflict could not be resolved (due to a breaking change), here
`TransitiveDependency` v2.0.0 is fully backwards compatible with v1.0.0 — same interface, same namespace.
The binding redirect lets the consumer tell the CLR: "when DirectDependencyA asks for v1.0.0, give it v2.0.0 instead."

## Structure

```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0 → TransitiveDependency [= 1.0.0]  (compiled against v1.0.0)
└── DirectDependencyLibraryB v1.0.0 → TransitiveDependency [= 2.0.0]  (compiled against v2.0.0)
```

Both direct dependency libraries are pre-compiled — you have no control over them.
`TransitiveDependency` v2.0.0 is **backwards compatible**: no API changes, no namespace changes.

## The Problem

Because both libraries are strongly named and compiled against specific versions of `TransitiveDependency`,
the consumer faces two conflicts:

1. **NuGet level**: `DirectLibraryA` requires `[= 1.0.0]` and `DirectLibraryB` requires `[= 2.0.0]`.
   NuGet cannot satisfy both constraints simultaneously → **NU1107 build error**.

2. **CLR level**: Even after pinning the version at the NuGet level, `DirectLibraryA.dll` has a
   compiled-in strong-name reference to `v1.0.0.0`. On machines where strong name version enforcement
   is active (e.g. production servers, CI environments, or machines without the developer bypass), the
   CLR will throw `FileLoadException` because only `v2.0.0.0` is present in the output.

## The Solution

The consumer resolves both conflicts:

**1. Explicit NuGet pin** in `ServiceConsumer.csproj` — override the conflicting transitive requirements
by pinning `TransitiveDependency` directly to `v2.0.0`:

```xml
<!-- Explicit pin required: DirectLibraryA wants [1.0.0], DirectLibraryB wants [2.0.0].
     NuGet cannot resolve the conflict on its own - we choose v2.0.0 because it is
     backwards compatible. The binding redirect in App.config then tells the CLR to
     accept v2.0.0 when DirectLibraryA asks for v1.0.0 at runtime. -->
<PackageReference Include="vp1143_TransitiveDependency_A" Version="2.0.0" />
```

**2. Binding redirect** in `App.config` — tell the CLR to redirect any request for v1.0.0 to v2.0.0:

```xml
<dependentAssembly>
  <assemblyIdentity name="vp1143_TransitiveDependency_A" culture="neutral" publicKeyToken="245a80169ac37524" />
  <bindingRedirect oldVersion="1.0.0.0-9.9.9.9" newVersion="2.0.0.0" />
</dependentAssembly>
```

This only works because v2.0.0 is backwards compatible. If it had breaking changes (like in `7h889t`),
no redirect would fix it — you'd need the library authors to recompile against the shared version.

## Why Strong Naming Is Involved

Strong naming gives assemblies an **exact identity**: name + version + public key token.
When `DirectLibraryA.dll` is compiled against `TransitiveDependency v1.0.0.0`, this exact identity
is baked into its IL manifest. At runtime, the CLR checks that the loaded assembly matches this identity.

> **Note on developer machines**: .NET Framework 4.5+ bypasses strong name verification for
> assemblies loaded from the local file system by default (`sn.exe -Vr *,*` is often set globally).
> On such machines the CLR version check is skipped and the runtime may work without the binding
> redirect. In stricter environments (production, CI, network paths, partial trust) the check IS
> enforced — which is where the binding redirect becomes essential.

## Why This Differs from Scenario 7h889t

| | 7h889t | vp1143 |
|---|---|---|
| TransitiveDependency v2.0.0 | Has breaking changes (namespace moved) | Backwards compatible |
| Without binding redirect | Runtime crash (`MissingMethodException`) | Works on dev machines (verification bypass) |
| With binding redirect to v2.0.0 | Still crashes (API incompatible) | Works everywhere |

## Testing

```bash
cd ServiceConsumer
dotnet build -c Release
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
```

Or run the automated test script from the repo root:
```bash
bash test-scenarios.sh
```

Scenario 1 removes the explicit pin → NuGet resolution failure (NU1107).
Scenario 2 restores the pin + keeps the binding redirect → both `DirectDepA` and `DirectDepB` succeed.
