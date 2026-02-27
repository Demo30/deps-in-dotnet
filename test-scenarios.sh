#!/bin/bash

echo "=========================================="
echo "Binding Redirect Test Scenarios (vp1143)"
echo "=========================================="
echo ""

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

CONSUMER_DIR="/c/repositories/_personalRepos/deps-in-dotnet/ServiceConsumer"
CSPROJ="$CONSUMER_DIR/ServiceConsumer/ServiceConsumer.csproj"
EXE="$CONSUMER_DIR/ServiceConsumer/bin/Release/net472/ServiceConsumer.exe"

restore_pin() {
    sed -i 's|<!-- PIN_DISABLED|<!-- PIN_DISABLED|' "$CSPROJ"
    sed -i 's|PIN_DISABLED_START -->||g' "$CSPROJ"
    sed -i 's|<!-- PIN_DISABLED_END -->||g' "$CSPROJ"
}

remove_pin() {
    sed -i 's|<PackageReference Include="vp1143_TransitiveDependency_A" Version="2.0.0" />|<!-- PIN_DISABLED_START --><PackageReference Include="vp1143_TransitiveDependency_A" Version="2.0.0" /><!-- PIN_DISABLED_END -->|g' "$CSPROJ"
}

build_and_run() {
    cd "$CONSUMER_DIR"
    dotnet restore > /dev/null 2>&1
    local restore_exit=$?

    if [ $restore_exit -ne 0 ]; then
        echo -e "${RED}Restore failed! (NuGet cannot resolve conflicting version constraints)${NC}"
        dotnet restore 2>&1 | grep "error NU"
        return 1
    fi

    dotnet build -c Release > /dev/null 2>&1
    if [ $? -ne 0 ]; then
        echo -e "${RED}Build failed!${NC}"
        return 1
    fi

    echo -e "${GREEN}Build successful!${NC}"
    echo "Running application..."
    echo ""
    "$EXE"
    if [ $? -eq 0 ]; then
        echo ""
        echo -e "${GREEN}✅ Application ran successfully!${NC}"
        return 0
    else
        echo ""
        echo -e "${RED}💥 Application crashed!${NC}"
        return 1
    fi
}

# -------------------------------------------------------------------
# Scenario 1: No explicit pin — NuGet cannot resolve the conflict
# -------------------------------------------------------------------
echo -e "${YELLOW}=== SCENARIO 1: No explicit version pin ===${NC}"
echo "DirectLibraryA requires TransitiveDependency [=1.0.0]"
echo "DirectLibraryB requires TransitiveDependency [=2.0.0]"
echo "Expected: 💥 NuGet resolution failure (NU1107)"
echo ""
remove_pin
build_and_run
RESULT1=$?

# Restore csproj
cd "/c/repositories/_personalRepos/deps-in-dotnet"
git checkout ServiceConsumer/ServiceConsumer/ServiceConsumer.csproj > /dev/null 2>&1

echo ""
echo ""
sleep 1

# -------------------------------------------------------------------
# Scenario 2: Explicit pin to v2.0.0 + binding redirect in App.config
# -------------------------------------------------------------------
echo -e "${YELLOW}=== SCENARIO 2: Explicit pin v2.0.0 + binding redirect ===${NC}"
echo "ServiceConsumer explicitly pins TransitiveDependency to v2.0.0"
echo "App.config binding redirect: 1.0.0-9.9.9.9 -> 2.0.0"
echo "Expected: ✅ Both methods succeed (v2.0.0 is backwards compatible)"
echo ""
build_and_run
RESULT2=$?

echo ""
echo ""
echo "=========================================="
echo "Test Summary"
echo "=========================================="

if [ $RESULT1 -ne 0 ]; then
    echo -e "Scenario 1 (no pin):              ${GREEN}✅ PASSED${NC} (NuGet conflict as expected)"
else
    echo -e "Scenario 1 (no pin):              ${RED}❌ FAILED${NC} (should have failed)"
fi

if [ $RESULT2 -eq 0 ]; then
    echo -e "Scenario 2 (pin + redirect):      ${GREEN}✅ PASSED${NC} (resolved as expected)"
else
    echo -e "Scenario 2 (pin + redirect):      ${RED}❌ FAILED${NC} (unexpected)"
fi

echo ""
echo "Binding redirect demonstration complete!"
