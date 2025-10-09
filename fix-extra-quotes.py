#!/usr/bin/env python3

import os
import re
from pathlib import Path

def fix_html_file(filepath):
    """Remove extra quote marks and spaces from attributes"""
    print(f"Processing: {filepath}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Fix attributes with extra quote and space at the end: src="/path" " -> src="/path"
    content = re.sub(r'(src|href|srcset)="([^"]+)"\s*"', r'\1="\2"', content)
    
    # Fix double quotes after paths: /path/"" -> /path"
    content = re.sub(r'/""\s*>', r'/">', content)
    content = re.sub(r'/""', r'/"', content)
    
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)

# Process all HTML files in content directory
content_dir = Path('/workspaces/website/content')
for html_file in content_dir.rglob('*.html'):
    fix_html_file(html_file)

print("Done! All extra quotes have been removed.")
