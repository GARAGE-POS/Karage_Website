#!/bin/bash

# Script to fix hardcoded /en/ and /ar/ URLs in content HTML files

echo "Fixing hardcoded language URLs in content files..."

# Fix English content files - replace /en/page/ with {{ "/page/" | relLangURL }}
find /workspaces/website/content/en -name "*.html" -type f | while read file; do
    echo "Processing EN: $file"
    sed -i 's|href="/en/\([^"]*\)"|href="{{ "/\1" \| relLangURL }}"|g' "$file"
done

# Fix Arabic content files - replace /ar/page/ with {{ "/page/" | relLangURL }}
find /workspaces/website/content/ar -name "*.html" -type f | while read file; do
    echo "Processing AR: $file"
    # First fix the double slash issue if it exists
    sed -i 's|href="/ar//|href="/ar/|g' "$file"
    # Then replace /ar/ paths
    sed -i 's|href="/ar/\([^"]*\)"|href="{{ "/\1" \| relLangURL }}"|g' "$file"
done

echo "Done! All language-specific URLs have been fixed."
