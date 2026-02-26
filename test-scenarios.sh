#!/bin/bash

echo "=========================================="
echo "Diamond Dependency Test Scenarios"
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

# Function to update csproj
update_csproj() {
    local depb_version=$1
    sed -i "s/7h889t_DirectLibrary_B\" Version=\"[0-9.]*\"/7h889t_DirectLibrary_B\" Version=\"$depb_version\"/g" "$CSPROJ"
}

# Function to build and run
build_and_run() {
    cd "$CONSUMER_DIR"
    dotnet clean > /dev/null 2>&1
    dotnet build -c Release > /dev/null 2>&1
    
    if [ $? -eq 0 ]; then
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
    else
        echo -e "${RED}Build failed!${NC}"
        return 1
    fi
}

# Scenario 1
echo -e "${YELLOW}=== SCENARIO 1: Both Dependencies Use v1.0.0 ===${NC}"
echo "Configuration: DirectLibraryA v1.0.0 + DirectLibraryB v1.0.0"
echo "Expected: ✅ Success (no diamond conflict)"
echo ""
update_csproj "1.0.0"
build_and_run
RESULT1=$?

echo ""
echo ""
sleep 2

# Scenario 2
echo -e "${YELLOW}=== SCENARIO 2: Diamond Dependency Conflict ===${NC}"
echo "Configuration: DirectLibraryA v1.0.0 + DirectLibraryB v2.0.0"
echo "Expected: 💥 Runtime crash (diamond conflict)"
echo ""
update_csproj "2.0.0"
build_and_run
RESULT2=$?

echo ""
echo ""
echo "=========================================="
echo "Test Summary"
echo "=========================================="

if [ $RESULT1 -eq 0 ]; then
    echo -e "Scenario 1 (v1.0.0): ${GREEN}✅ PASSED${NC} (worked as expected)"
else
    echo -e "Scenario 1 (v1.0.0): ${RED}❌ FAILED${NC} (unexpected)"
fi

if [ $RESULT2 -ne 0 ]; then
    echo -e "Scenario 2 (v2.0.0): ${GREEN}✅ PASSED${NC} (crashed as expected)"
else
    echo -e "Scenario 2 (v2.0.0): ${RED}❌ FAILED${NC} (should have crashed)"
fi

echo ""
echo "Diamond dependency demonstration complete!"
