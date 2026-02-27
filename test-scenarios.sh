#!/bin/bash

echo "=========================================="
echo "Binding Redirect Test Scenarios (vp1143)"
echo "=========================================="
echo ""

GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

CONSUMER_DIR="/c/repositories/_personalRepos/deps-in-dotnet/ServiceConsumer"
APP_CONFIG="$CONSUMER_DIR/ServiceConsumer/App.config"
EXE="$CONSUMER_DIR/ServiceConsumer/bin/Release/net472/ServiceConsumer.exe"

build_and_run() {
    cd "$CONSUMER_DIR"
    dotnet build -c Release > /dev/null 2>&1
    if [ $? -ne 0 ]; then
        echo -e "${RED}Build failed!${NC}"
        return 1
    fi
    echo -e "${GREEN}Build successful!${NC}"
    echo "Running application..."
    echo ""
    "$EXE"
    local exit_code=$?
    echo ""
    if [ $exit_code -eq 0 ]; then
        echo -e "${GREEN}✅ Application ran successfully!${NC}"
    else
        echo -e "${RED}💥 Application crashed!${NC}"
    fi
    return $exit_code
}

# -------------------------------------------------------------------
# Scenario 1: No binding redirect — CLR cannot match v1.0.0 reference
# -------------------------------------------------------------------
echo -e "${YELLOW}=== SCENARIO 1: No binding redirect ===${NC}"
echo "DirectLibraryA.dll compiled against TransitiveDependency v1.0.0.0 (strong name)"
echo "Output folder contains TransitiveDependency v2.0.0.0 (NuGet picked highest)"
echo "No redirect telling the CLR how to resolve the mismatch"
echo "Expected: 💥 FileLoadException (manifest definition does not match)"
echo ""

cat > "$APP_CONFIG" << 'EOF'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
</configuration>
EOF

build_and_run
RESULT1=$?

echo ""
echo ""
sleep 1

# -------------------------------------------------------------------
# Scenario 2: Correct binding redirect — CLR resolves v1.0.0 → v2.0.0
# -------------------------------------------------------------------
echo -e "${YELLOW}=== SCENARIO 2: Correct binding redirect ===${NC}"
echo "Same setup, but App.config redirects all v1.0.0-v9.9.9.9 requests to v2.0.0"
echo "v2.0.0 is backwards compatible — same interface, no breaking changes"
echo "Expected: ✅ Both methods succeed"
echo ""

cat > "$APP_CONFIG" << 'EOF'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="vp1143_TransitiveDependency_A" culture="neutral" publicKeyToken="245a80169ac37524" />
        <bindingRedirect oldVersion="1.0.0.0-9.9.9.9" newVersion="2.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
EOF

build_and_run
RESULT2=$?

echo ""
echo ""
echo "=========================================="
echo "Test Summary"
echo "=========================================="

if [ $RESULT1 -ne 0 ]; then
    echo -e "Scenario 1 (no redirect):      ${GREEN}✅ PASSED${NC} (crashed as expected)"
else
    echo -e "Scenario 1 (no redirect):      ${RED}❌ FAILED${NC} (should have crashed)"
fi

if [ $RESULT2 -eq 0 ]; then
    echo -e "Scenario 2 (with redirect):    ${GREEN}✅ PASSED${NC} (resolved as expected)"
else
    echo -e "Scenario 2 (with redirect):    ${RED}❌ FAILED${NC} (unexpected)"
fi

echo ""
echo "Binding redirect demonstration complete!"
