#!/bin/bash

# This script removes /Karage_Website/ prefix from all paths in HTML files
# This makes paths relative so Hugo's baseURL can prepend them correctly

echo "Removing /Karage_Website/ prefix from all paths..."

find /workspaces/website/content -name "*.html" -type f | while read file; do
    echo "Processing: $file"
    # Remove /Karage_Website/ prefix from src, href, and srcset attributes
    sed -i 's|="/Karage_Website/|="/|g' "$file"
    # Also fix any malformed template syntax
    sed -i 's|src="{{ "|\src="/|g' "$file"
    sed -i 's|href="{{ "|\href="/|g' "$file"
    sed -i 's|srcset="{{ "|srcset="/|g' "$file"
    sed -i 's|" | relURL }}"|"|g' "$file"
    sed -i 's|" | rellangurl }}"|"|g' "$file"
done

echo "Done! All /Karage_Website/ prefixes have been removed."
