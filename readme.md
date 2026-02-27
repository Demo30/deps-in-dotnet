# Scenario vp1143 - Binding Redirect as the Correct Solution

## Summary

This scenario demonstrates a **good use of binding redirects** to resolve a diamond dependency
problem where `TransitiveDependency` v2.0.0 is fully backwards compatible with v1.0.0.

Two pre-compiled vendor libraries both depend on `TransitiveDependency`, but at different versions.
You cannot recompile either library. The binding redirect lets the consumer tell the CLR:
"when `DirectDependencyA` asks for v1.0.0, give it v2.0.0 — they are compatible."

## Structure

```
ServiceConsumer (net472)
├── DirectDependencyLibraryA v1.0.0 → TransitiveDependency v1.0.0  (compiled, strong-named)
└── DirectDependencyLibraryB v1.0.0 → TransitiveDependency v2.0.0  (compiled, strong-named)
```

`TransitiveDependency` v2.0.0 is **backwards compatible**: same interface, same namespace.

## The Two Conflicts

### 1. NuGet level (build time)

`DirectLibraryA`'s nuspec requires `TransitiveDependency [= 1.0.0]` and `DirectLibraryB`'s
nuspec requires `[= 2.0.0]`. NuGet cannot satisfy both simultaneously → **NU1107**.

**Fix:** explicitly pin `TransitiveDependency` to `2.0.0` in `ServiceConsumer.csproj`,
overriding both transitive constraints.

### 2. CLR level (runtime)

Even after the NuGet pin, `DirectLibraryA.dll` has a compiled-in strong-name reference to
`v1.0.0.0`. NuGet copies `v2.0.0.0` to the output (highest wins). At runtime the CLR finds
`v2.0.0.0` on disk but `DirectLibraryA` declares it needs `v1.0.0.0` exactly →
**FileLoadException: manifest definition does not match**.

**Fix:** add a binding redirect in `App.config` telling the CLR to serve `v2.0.0` for any
request in the range `1.0.0.0–9.9.9.9`.

## A Note on AutoGenerateBindingRedirects

In real .NET Framework exe projects, MSBuild has `AutoGenerateBindingRedirects` **enabled by
default**. It detects version conflicts at build time and silently writes the binding redirect
into the output `.exe.config` on your behalf — which is why version conflicts often resolve
transparently in day-to-day development without the developer ever writing a redirect manually.

This scenario has `AutoGenerateBindingRedirects` **disabled** in `ServiceConsumer.csproj` so that
the underlying CLR conflict is visible and the manually-written redirect in `App.config` is what
actually fixes it. This mirrors scenarios where auto-generation is not available: classic
non-SDK projects, web applications (`web.config`), or Windows services where the auto-generated
config may not be deployed correctly.

## Why Strong Naming Is Involved

Strong naming gives assemblies an exact identity: name + **version** + public key token.
When `DirectLibraryA.dll` is compiled against `TransitiveDependency v1.0.0.0`, this identity
is baked into its IL manifest. At runtime the CLR checks the loaded assembly matches this
identity exactly. Without a binding redirect to bridge the gap, the mismatch causes a
`FileLoadException`.

## Why This Differs from Scenario 7h889t

| | 7h889t | vp1143 |
|---|---|---|
| TransitiveDependency v2.0.0 | Breaking change (namespace moved) | Backwards compatible |
| Binding redirect possible fix? | No — API incompatible regardless | Yes |
| Outcome with correct redirect | Still crashes (MissingMethodException) | Both methods succeed |

## Testing

Run the automated test script from the repo root:

```bash
bash test-scenarios.sh
```

**Scenario 1** — App.config with no redirect → `FileLoadException` at startup of
`DirectDependencyA` (CLR rejects v2.0.0.0 when manifest declares v1.0.0.0).

**Scenario 2** — App.config with correct redirect (`1.0.0.0–9.9.9.9 → 2.0.0.0`) →
both `DirectDepA` and `DirectDepB` succeed.

Or run manually:

```bash
cd ServiceConsumer
dotnet build -c Release
ServiceConsumer/bin/Release/net472/ServiceConsumer.exe
```
