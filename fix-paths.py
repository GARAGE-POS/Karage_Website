#!/usr/bin/env python3

import os
import re
from pathlib import Path

def fix_html_file(filepath):
    """Remove /Karage_Website/ prefix and fix broken template syntax"""
    print(f"Processing: {filepath}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Remove /Karage_Website/ prefix from paths
    content = re.sub(r'="/Karage_Website/', r'="/', content)
    
    # Fix broken template syntax from our earlier sed commands
    content = re.sub(r'src=\{\{ "([^"]+)" \| relURL \}\}', r'src="/\1"', content)
    content = re.sub(r'href=\{\{ "([^"]+)" \| relURL \}\}', r'href="/\1"', content)
    content = re.sub(r'srcset=\{\{ "([^"]+)" \| relURL \}\}', r'srcset="/\1"', content)
    content = re.sub(r'href=\{\{ "([^"]+)" \| relLangURL \}\}', r'href="/\1"', content)
    
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)

# Process all HTML files in content directory
content_dir = Path('/workspaces/website/content')
for html_file in content_dir.rglob('*.html'):
    fix_html_file(html_file)

print("Done! All paths have been fixed.")
