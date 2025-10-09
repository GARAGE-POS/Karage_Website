#!/bin/bash

# Script to fix all /Karage_Website/ paths to relative paths in content HTML files
# Since Hugo doesn't process template syntax in content HTML, we need to use relative paths

echo "Converting /Karage_Website/ paths to relative paths in content files..."

# The baseURL is https://garage-pos.github.io/Karage_Website/
# So we just need to remove /Karage_Website/ and Hugo will prepend the baseURL automatically

find /workspaces/website/content -name "*.html" -type f | while read file; do
    echo "Processing: $file"
    
    # Replace src="{{ "path" | relURL }}" back to src="/Karage_Website/path"
    # Then replace /Karage_Website/ with empty string (relative path)
    
    # First, revert the template syntax we added (since it doesn't work in content files)
    sed -i 's|src="{{ "\([^"]*\)" | relURL }}"|src="/Karage_Website/\1"|g' "$file"
    sed -i 's|href="{{ "\([^"]*\)" | relURL }}"|href="/Karage_Website/\1"|g' "$file"
    sed -i 's|srcset="{{ "\([^"]*\)" | relURL }}"|srcset="/Karage_Website/\1"|g' "$file"
    
    # Now fix the paths - keep /Karage_Website/ as is since that's what the baseURL is set to
done

echo "Done!"
