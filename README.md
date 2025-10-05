# Karage Website

This is the official Karage website built with [Hugo](https://gohugo.io/), a fast and flexible static site generator written in Go.

## What is Hugo?

Hugo is a static site generator that takes your content (written in Markdown) and templates (HTML layouts) and generates a complete static website. Static sites are fast, secure, and easy to deploy because they're just HTML, CSS, and JavaScript files with no database or server-side processing required.

## Prerequisites

This project runs in a dev container that includes all necessary tools pre-installed:
- **Hugo** - The static site generator
- **Git** - Version control
- **Node.js & npm** - For any JavaScript dependencies

If you're running this outside the dev container, you'll need to install Hugo separately: https://gohugo.io/installation/

## Project Structure

```
.
├── archetypes/          # Content templates for new pages
├── assets/              # Assets that need processing (SASS, JS, images)
├── content/             # Your website content (Markdown files)
│   ├── en/             # English content
│   └── ar/             # Arabic content
├── data/                # Data files (JSON, YAML, TOML) for use in templates
├── i18n/                # Translation files for internationalization
├── layouts/             # HTML templates that define page structure
│   └── _default/       # Default layout templates
├── static/              # Static files (copied as-is to output)
│   ├── css/
│   ├── fonts/
│   ├── images/
│   └── js/
├── themes/              # Hugo themes (if using any)
├── public/              # Generated website (created by Hugo, don't edit!)
└── hugo.toml           # Hugo configuration file
```

## Getting Started

### 1. Start the Development Server

The easiest way to preview your site locally is to run the Hugo development server:

```bash
hugo server -D --bind 0.0.0.0 --baseURL http://localhost:1313
```

Or use the pre-configured VS Code task:
- Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
- Type "Run Task"
- Select "Hugo: Serve"

This will:
- Build your site
- Start a local web server at http://localhost:1313
- Watch for changes and automatically rebuild
- Live reload your browser when files change
- Include draft content (`-D` flag)

**Options explained:**
- `-D` or `--buildDrafts` - Include draft content
- `--bind 0.0.0.0` - Bind to all network interfaces (needed for dev containers)
- `--baseURL` - Set the base URL for the site

### 2. View the Website

Open your browser and navigate to:
```
http://localhost:1313
```

The site will automatically reload whenever you make changes to your files.

### 3. Stop the Server

Press `Ctrl+C` in the terminal where Hugo is running.

## Common Development Tasks

### Creating New Content

To create a new page:

```bash
hugo new content/en/mypage/index.md
```

This creates a new content file using the template from `archetypes/default.md`.

### Building for Production

To build the final static site:

```bash
hugo --cleanDestinationDir
```

Or with minification for production:

```bash
hugo --minify --cleanDestinationDir
```

Or use VS Code tasks:
- "Hugo: Build" - Standard build
- "Hugo: Build (Production)" - Minified build for production

The generated site will be in the `public/` directory, ready to deploy.

### Cleaning Build Files

To remove generated files:

```bash
rm -rf public resources
```

Or use the VS Code task: "Hugo: Clean"

## Understanding Hugo Concepts

### Front Matter

Each content file starts with "front matter" - metadata about the page in YAML, TOML, or JSON format:

```yaml
---
title: "My Page Title"
date: 2025-10-05
draft: false
description: "Page description for SEO"
---
```

### Layouts and Templates

Hugo uses Go templates to generate HTML. Templates are in the `layouts/` directory:
- `baseof.html` - The base template that other templates extend
- `single.html` - Template for individual pages
- `list.html` - Template for list pages (categories, tags)
- `home.html` - Template for the homepage

### Static vs Assets

- **static/** - Files copied as-is to the output (images, fonts, existing CSS/JS)
- **assets/** - Files that need processing (SASS to CSS, JS bundling, image optimization)

### Multilingual Support

This site supports multiple languages (English and Arabic):
- Content is organized by language in `content/en/` and `content/ar/`
- Translations are in `i18n/` directory
- Configuration in `hugo.toml` defines available languages

## Useful Hugo Commands

```bash
# Start development server
hugo server -D

# Build site
hugo

# Build with minification
hugo --minify

# Check Hugo version
hugo version

# Get help
hugo help

# Create new content
hugo new content/en/blog/my-post.md
```

## VS Code Tasks Available

This project includes pre-configured tasks accessible via `Ctrl+Shift+P` → "Run Task":

1. **Hugo: Serve** - Development server with live reload
2. **Hugo: Serve (Fast Render)** - Faster rendering (only rebuilds changed content)
3. **Hugo: Build** - Standard production build
4. **Hugo: Build (Production)** - Minified production build
5. **Hugo: Clean** - Remove generated files

## Tips for Development

1. **Live Reload**: Keep `hugo server` running while you work - changes appear instantly in your browser
2. **Draft Content**: Use `draft: true` in front matter for work-in-progress pages
3. **Fast Render**: Use fast render mode when working on a single page for faster rebuilds
4. **Check Errors**: Watch the terminal output from `hugo server` for build errors
5. **Content Organization**: Keep content organized by language and section

## Troubleshooting

### Port Already in Use
If port 1313 is already in use, specify a different port:
```bash
hugo server -D --bind 0.0.0.0 --baseURL http://localhost:1313 --port 1314
```

### Changes Not Appearing
- Ensure `hugo server` is running
- Check the terminal for build errors
- Hard refresh your browser (`Ctrl+Shift+R` or `Cmd+Shift+R`)
- Clear the `public/` and `resources/` directories and rebuild

### Template Errors
- Check the terminal output for specific error messages
- Verify your Go template syntax in layout files
- Ensure all referenced variables exist in your front matter

## Learning Resources

- [Hugo Official Documentation](https://gohugo.io/documentation/)
- [Hugo Quick Start Guide](https://gohugo.io/getting-started/quick-start/)
- [Hugo Templates Introduction](https://gohugo.io/templates/introduction/)
- [Content Management in Hugo](https://gohugo.io/content-management/)
- [Hugo Directory Structure](https://gohugo.io/getting-started/directory-structure/)

## Deployment

Once you've built your site with `hugo --minify`, the `public/` directory contains your complete static website. You can deploy this directory to:
- Netlify
- Vercel
- GitHub Pages
- AWS S3
- Any static hosting service

## Contributing

1. Make your changes in a new branch
2. Test locally with `hugo server -D`
3. Build the site with `hugo --minify` to ensure no errors
4. Commit your changes
5. Create a pull request

## Questions?

If you're new to Hugo and have questions:
1. Check the [Hugo Documentation](https://gohugo.io/documentation/)
2. Look at existing content files and layouts in this project as examples
3. Ask the team for help

---

**Current Branch**: move_to_hugo  
**Repository**: GARAGE-POS/Karage_Website
