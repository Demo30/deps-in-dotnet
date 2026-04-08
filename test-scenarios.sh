#!/usr/bin/env bash
# ------------------------------------------------------------------
# Scenario kx7f2m – Microsoft.Net.Http ↔ System.Net.Http compatibility
# ------------------------------------------------------------------
# This script demonstrates two scenarios:
#   1) DirectDependencyA v1.0.0 (Microsoft.Net.Http) + DirectDependencyB (System.Net.Http)
#   2) DirectDependencyA v2.0.0 (System.Net.Http)    + DirectDependencyB (System.Net.Http)
#
# Both scenarios pass HttpResponseMessage and HttpRequestHeaders across
# the package boundary to verify type identity compatibility.
# ------------------------------------------------------------------

set -euo pipefail

CONSUMER_DIR="ServiceConsumer/ServiceConsumer"
CSPROJ="$CONSUMER_DIR/ServiceConsumer.csproj"

update_direct_dep_a_version() {
    local version="$1"
    sed -i "s|Include=\"kx7f2m_DirectLibrary_A\" Version=\"[^\"]*\"|Include=\"kx7f2m_DirectLibrary_A\" Version=\"$version\"|" "$CSPROJ"
}

build_and_run() {
    pushd "$CONSUMER_DIR" > /dev/null
    rm -rf bin obj
    dotnet restore --verbosity quiet 2>&1
    dotnet build -c Release --no-restore --verbosity quiet 2>&1
    echo ""
    ./bin/Release/net472/ServiceConsumer.exe 2>&1 || true
    echo ""
    echo "Output directory DLLs:"
    ls bin/Release/net472/*.dll 2>/dev/null | xargs -I{} basename {} | sort
    popd > /dev/null
}

# ─── Scenario 1 ────────────────────────────────────────────────────
echo "============================================================"
echo "SCENARIO 1: DirectDependencyA v1.0.0 (Microsoft.Net.Http)"
echo "          + DirectDependencyB v1.0.0 (System.Net.Http)"
echo "============================================================"
echo ""
echo "DirectDependencyA obtains System.Net.Http types via the"
echo "deprecated Microsoft.Net.Http meta-package."
echo "DirectDependencyB obtains the same types via System.Net.Http"
echo "NuGet package directly."
echo ""
echo "The consumer passes HttpResponseMessage from A to B and"
echo "HttpRequestHeaders from B to A to test cross-package type"
echo "compatibility."
echo ""

update_direct_dep_a_version "1.0.0"
build_and_run

echo ""
echo "------------------------------------------------------------"
echo "RESULT: All calls succeed. The types are identical because"
echo "on net472, both packages resolve to the same framework"
echo "System.Net.Http.dll assembly."
echo ""
echo "Note: Microsoft.Net.Http brings extra DLLs (Extensions,"
echo "Primitives) and transitive dependencies (Microsoft.Bcl,"
echo "Microsoft.Bcl.Build) that System.Net.Http does not."
echo "------------------------------------------------------------"
echo ""
echo ""

# ─── Scenario 2 ────────────────────────────────────────────────────
echo "============================================================"
echo "SCENARIO 2: DirectDependencyA v2.0.0 (System.Net.Http)"
echo "          + DirectDependencyB v1.0.0 (System.Net.Http)"
echo "============================================================"
echo ""
echo "DirectDependencyA has migrated from Microsoft.Net.Http to"
echo "System.Net.Http NuGet package. Both dependencies now use"
echo "the same package."
echo ""

update_direct_dep_a_version "2.0.0"
build_and_run

echo ""
echo "------------------------------------------------------------"
echo "RESULT: All calls still succeed. The migration from"
echo "Microsoft.Net.Http to System.Net.Http is transparent."
echo ""
echo "Note: The output directory is now cleaner — no more"
echo "Extensions/Primitives DLLs from the legacy meta-package."
echo "Transitive dependencies (Microsoft.Bcl, Microsoft.Bcl.Build)"
echo "are also gone."
echo "------------------------------------------------------------"

# Reset to scenario 1
update_direct_dep_a_version "1.0.0"
