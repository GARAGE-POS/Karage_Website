# URL Fixes for GitHub Pages Deployment

## Problem
The website was experiencing 404 errors when deployed to GitHub Pages because URLs were not properly prefixed with the base path `/Karage_Website/`.

## Solution
Enabled Hugo's `canonifyURLs` configuration and updated all layout templates to use Hugo's URL functions.

## Changes Made

### 1. Hugo Configuration (`hugo.toml`)
- Added `canonifyURLs = true` to ensure all URLs are converted to absolute URLs with the baseURL

### 2. Layout Files (Hugo Templates)
Updated the following files to use Hugo template functions:

- **`layouts/_default/baseof.html`**
  - Changed CSS/JS paths from `/Karage_Website/css/...` to `{{ "css/..." | relURL }}`
  - Changed image paths from `/Karage_Website/images/...` to `{{ "images/..." | relURL }}`

- **`layouts/partials/navbar-en.html`**
  - Changed `/en/home` to `{{ "/home/" | relLangURL }}`
  - Changed image paths to use `{{ "images/..." | relURL }}`

- **`layouts/partials/navbar-ar.html`**
  - Changed `/ar/home` to `{{ "/home/" | relLangURL }}`
  - Changed image paths to use `{{ "images/..." | relURL }}`

- **`layouts/partials/footer-en.html` and `footer-ar.html`**
  - Changed all image paths to use `{{ "images/..." | relURL }}`

### 3. Content HTML Files
Cleaned up all content HTML files in `content/en/` and `content/ar/`:
- Removed `/Karage_Website/` prefixes from all paths
- Fixed broken template syntax remnants
- Removed extra quotes and malformed attributes

## Result
All URLs now correctly resolve to:
- `https://garage-pos.github.io/Karage_Website/css/...`
- `https://garage-pos.github.io/Karage_Website/images/...`
- `https://garage-pos.github.io/Karage_Website/en/...`
- `https://garage-pos.github.io/Karage_Website/ar/...`

This ensures the site works correctly when deployed to GitHub Pages at `https://garage-pos.github.io/Karage_Website/`.

## Scripts Created
Three Python scripts were created to automate the fixes:
1. `fix-paths.py` - Removed /Karage_Website/ prefixes and fixed template syntax
2. `fix-broken-syntax.py` - Cleaned up remaining broken Hugo template syntax
3. `fix-extra-quotes.py` - Removed extra quotes from HTML attributes

## Testing
To test locally:
```bash
hugo server -D --bind 0.0.0.0 --baseURL http://localhost:1313
```

To build for production:
```bash
hugo --minify --cleanDestinationDir
```

The `public/` directory can now be deployed to GitHub Pages and all links will work correctly.
