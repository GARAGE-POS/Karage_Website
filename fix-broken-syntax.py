#!/usr/bin/env python3

import os
import re
from pathlib import Path

def fix_html_file(filepath):
    """Clean up all broken template syntax and malformed attributes"""
    print(f"Processing: {filepath}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Fix broken href attributes with template syntax remnants
    content = re.sub(r'href="//([^"]+)" \| relLangURL \}\}', r'href="/\1"', content)
    content = re.sub(r'href=""/([^"]+)""', r'href="/\1"', content)
    
    # Fix broken src attributes with template syntax remnants  
    content = re.sub(r'src="/([^"]+)" \| relURL \}\}', r'src="/\1"', content)
    content = re.sub(r'srcset="/([^"]+)" \| relURL \}\}', r'srcset="/\1"', content)
    
    # Fix any remaining {{ }} template syntax
    content = re.sub(r'\{\{ "([^"]+)" \| relURL \}\}', r'"/\1"', content)
    content = re.sub(r'\{\{ "([^"]+)" \| relLangURL \}\}', r'"/\1"', content)
    
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)

# Process all HTML files in content directory
content_dir = Path('/workspaces/website/content')
for html_file in content_dir.rglob('*.html'):
    fix_html_file(html_file)

print("Done! All broken syntax has been fixed.")
