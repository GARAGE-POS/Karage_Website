#!/bin/bash

# Script to fix hardcoded /Karage_Website/ URLs in HTML files
# This will replace them with proper Hugo template syntax

echo "Fixing hardcoded URLs in content files..."

# Find all HTML files in content directory and replace /Karage_Website/ paths
find /workspaces/website/content -name "*.html" -type f | while read file; do
    echo "Processing: $file"
    
    # Replace src="/Karage_Website/images/ with src="{{ "images/
    # Replace href="/Karage_Website/images/ with href="{{ "images/
    # Replace src="/Karage_Website/css/ with src="{{ "css/
    # Replace href="/Karage_Website/css/ with href="{{ "css/
    # Replace src="/Karage_Website/js/ with src="{{ "js/
    # etc.
    
    # Note: We need to add | relURL }}" at the end of each path
    # This is complex in sed, so we'll do it in multiple steps
    
    sed -i 's|src="/Karage_Website/\([^"]*\)"|src="{{ "\1" \| relURL }}"|g' "$file"
    sed -i 's|href="/Karage_Website/\([^"]*\)"|href="{{ "\1" \| relURL }}"|g' "$file"
    sed -i 's|srcset="/Karage_Website/\([^"]*\)"|srcset="{{ "\1" \| relURL }}"|g' "$file"
done

echo "Done! All files have been processed."
