# Azure Deployment Guide for Karage Website

This guide explains how to deploy the Karage Hugo static website to Azure using GitHub Actions with Linux and Apache.

## Table of Contents
1. [Azure Setup](#azure-setup)
2. [GitHub Repository Setup](#github-repository-setup)
3. [GitHub Secrets Configuration](#github-secrets-configuration)
4. [Deployment Methods](#deployment-methods)
5. [Troubleshooting](#troubleshooting)

---

## Azure Setup

### Option 1: Azure App Service (Recommended for Simplicity)

Azure App Service provides managed hosting with built-in deployment support.

#### Step 1: Create an Azure App Service

1. **Log in to Azure Portal**: https://portal.azure.com
2. **Create a new Web App**:
   - Click "Create a resource" → "Web App"
   - **Subscription**: Select your subscription
   - **Resource Group**: Create new or select existing
   - **Name**: Choose a unique name (e.g., `karage-website`)
   - **Publish**: Code
   - **Runtime stack**: PHP 8.x or Node.js (for static sites)
   - **Operating System**: Linux
   - **Region**: Choose closest to your users
   - **App Service Plan**: Choose based on your needs (B1 or higher recommended)

3. **Configure Apache** (if needed):
   - After creation, go to Configuration → General Settings
   - Startup Command: (leave empty for default Apache setup)

4. **Download Publish Profile**:
   - Go to your Web App in Azure Portal
   - Click "Get publish profile" in the Overview page
   - Save the downloaded `.PublishSettings` file (you'll need its contents for GitHub Secrets)

#### Step 2: Configure Web App Settings

1. Go to **Configuration** → **Application settings**
2. Add any environment variables if needed
3. Go to **Configuration** → **General settings**
4. Set the default document to `index.html`

### Option 2: Azure Virtual Machine with Apache

For more control, you can use an Azure VM:

1. **Create Ubuntu VM**:
   - Create a resource → Virtual Machine
   - Choose Ubuntu 20.04 or 22.04 LTS
   - Set up SSH key or password authentication
   - Open ports 80 (HTTP) and 443 (HTTPS)

2. **Install Apache**:
   ```bash
   sudo apt update
   sudo apt install apache2 -y
   sudo systemctl enable apache2
   sudo systemctl start apache2
   ```

3. **Configure Apache**:
   ```bash
   sudo nano /etc/apache2/sites-available/karage.conf
   ```
   
   Add:
   ```apache
   <VirtualHost *:80>
       ServerName your-domain.com
       ServerAlias www.your-domain.com
       DocumentRoot /var/www/karage
       
       <Directory /var/www/karage>
           AllowOverride All
           Require all granted
           Options -Indexes +FollowSymLinks
       </Directory>
       
       ErrorLog ${APACHE_LOG_DIR}/karage_error.log
       CustomLog ${APACHE_LOG_DIR}/karage_access.log combined
   </VirtualHost>
   ```

4. **Enable the site and mod_rewrite**:
   ```bash
   sudo a2enmod rewrite
   sudo a2enmod headers
   sudo a2enmod expires
   sudo a2enmod deflate
   sudo a2ensite karage.conf
   sudo systemctl reload apache2
   ```

5. **Create deployment directory**:
   ```bash
   sudo mkdir -p /var/www/karage
   sudo chown -R $USER:www-data /var/www/karage
   sudo chmod -R 755 /var/www/karage
   ```

---

## GitHub Repository Setup

### 1. Push your code to GitHub

If you haven't already:

```bash
git add .
git commit -m "Add Azure deployment configuration"
git push origin main
```

### 2. Enable GitHub Actions

GitHub Actions should be enabled by default. The workflow file at `.github/workflows/azure-deploy.yml` will automatically trigger on push to the main branch.

---

## GitHub Secrets Configuration

You need to add secrets to your GitHub repository for secure deployment.

### Navigate to Repository Secrets

1. Go to your GitHub repository
2. Click **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**

### Required Secrets

#### For Azure App Service Deployment:

**Secret 1: `AZURE_WEBAPP_NAME`**
- Value: Your Azure Web App name (e.g., `karage-website`)

**Secret 2: `AZURE_WEBAPP_PUBLISH_PROFILE`**
- Value: Contents of the `.PublishSettings` file you downloaded
- Open the file in a text editor and copy the entire XML content

#### For FTP Deployment (Alternative):

If using FTP instead of App Service:

**Secret 1: `FTP_SERVER`**
- Value: FTP server address (e.g., `ftp.yoursite.com`)

**Secret 2: `FTP_USERNAME`**
- Value: FTP username

**Secret 3: `FTP_PASSWORD`**
- Value: FTP password

#### For SSH/VM Deployment (Alternative):

If deploying to a VM via SSH:

**Secret 1: `SSH_PRIVATE_KEY`**
- Value: Your SSH private key (the entire key including headers)
- Generate if needed: `ssh-keygen -t rsa -b 4096 -C "github-actions"`

**Secret 2: `REMOTE_HOST`**
- Value: VM IP address or hostname

**Secret 3: `REMOTE_USER`**
- Value: SSH username (e.g., `azureuser`)

**Secret 4: `REMOTE_TARGET`**
- Value: Deployment path (e.g., `/var/www/karage/`)

---

## Deployment Methods

The workflow file includes three deployment options. Choose one and comment out the others.

### Method 1: Azure Web App Deploy (Default)

Already configured in the workflow. Just ensure the secrets are set:
- `AZURE_WEBAPP_NAME`
- `AZURE_WEBAPP_PUBLISH_PROFILE`

### Method 2: FTP Deploy

1. Uncomment the "Deploy via FTP" section in `.github/workflows/azure-deploy.yml`
2. Comment out the "Deploy to Azure Web App" section
3. Set the required FTP secrets

### Method 3: SSH Deploy

1. Uncomment the "Deploy via SSH" section in `.github/workflows/azure-deploy.yml`
2. Comment out the "Deploy to Azure Web App" section
3. Set the required SSH secrets
4. Add your GitHub Actions public key to the VM:
   ```bash
   # On the VM:
   mkdir -p ~/.ssh
   echo "YOUR_PUBLIC_KEY" >> ~/.ssh/authorized_keys
   chmod 600 ~/.ssh/authorized_keys
   ```

---

## Updating Production Configuration

Before deploying, update your `hugo.toml` to use your production domain:

```toml
baseURL = 'https://your-domain.com'
```

You can also set this via environment variables in the workflow or Azure configuration.

---

## Testing the Deployment

### 1. Trigger Deployment

Push a commit to the main branch:

```bash
git add .
git commit -m "Test deployment"
git push origin main
```

Or manually trigger from GitHub:
- Go to **Actions** tab
- Select "Deploy to Azure" workflow
- Click "Run workflow"

### 2. Monitor Progress

- Go to the **Actions** tab in your GitHub repository
- Watch the workflow execution
- Check for any errors in the build or deploy steps

### 3. Verify the Website

- Visit your Azure Web App URL (e.g., `https://karage-website.azurewebsites.net`)
- Test both English (`/en/`) and Arabic (`/ar/`) versions
- Check that all static assets load correctly

---

## Custom Domain Setup (Optional)

### 1. In Azure Portal

1. Go to your Web App → **Custom domains**
2. Click **Add custom domain**
3. Enter your domain name
4. Follow the verification steps (add CNAME or TXT records to your DNS)

### 2. Enable HTTPS

1. Go to **TLS/SSL settings** → **Private Key Certificates**
2. Add a certificate (you can use Azure's free managed certificate)
3. Go to **Custom domains** and bind the certificate

### 3. Update hugo.toml

```toml
baseURL = 'https://your-custom-domain.com'
```

---

## Troubleshooting

### Build Fails

**Issue**: Hugo build fails in GitHub Actions

**Solutions**:
- Check the Actions log for specific errors
- Ensure all content files are valid
- Test locally: `hugo --minify --cleanDestinationDir`

### Deployment Fails

**Issue**: Deployment step fails

**Solutions**:
- Verify GitHub secrets are correctly set
- Check that the publish profile is not expired
- Ensure the Azure Web App is running
- Check Azure Web App logs in the portal

### 404 Errors

**Issue**: Pages return 404 errors

**Solutions**:
- Verify `.htaccess` file is in the web root
- Check that `mod_rewrite` is enabled (for Apache)
- Ensure the Web App is configured to use the correct document root
- Check file permissions on VM deployments

### Assets Not Loading

**Issue**: CSS, JS, or images not loading

**Solutions**:
- Check `baseURL` in `hugo.toml` matches your domain
- Verify static files are in the `public/` directory after build
- Check browser console for CORS or path errors
- Ensure Apache is configured to serve static files

### Language Routing Issues

**Issue**: Language redirects not working

**Solutions**:
- Verify `.htaccess` rules are being applied
- Check that `mod_rewrite` is enabled
- Test language detection manually
- Review Apache error logs

---

## Maintenance

### Updating Content

1. Edit content files in `content/en/` or `content/ar/`
2. Commit and push changes
3. GitHub Actions will automatically rebuild and redeploy

### Updating Dependencies

- Hugo version: Update in `.github/workflows/azure-deploy.yml`
- Apache modules: SSH to VM and install/update as needed

### Monitoring

- **Azure Portal**: Monitor Web App metrics (CPU, memory, requests)
- **GitHub Actions**: Review deployment history
- **Apache Logs**: Check `/var/log/apache2/` on VM deployments

---

## Security Checklist

- [ ] HTTPS enabled with valid certificate
- [ ] Security headers configured (in `.htaccess`)
- [ ] Directory browsing disabled
- [ ] Secrets properly stored in GitHub (never in code)
- [ ] Azure resource access restricted (firewall rules, NSG)
- [ ] Regular updates applied to Apache and system packages
- [ ] Monitoring and alerts configured

---

## Additional Resources

- [Hugo Documentation](https://gohugo.io/documentation/)
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Apache Documentation](https://httpd.apache.org/docs/)

---

## Support

For issues specific to:
- **Hugo**: Check [Hugo Discourse](https://discourse.gohugo.io/)
- **Azure**: See [Azure Support](https://azure.microsoft.com/en-us/support/)
- **GitHub Actions**: Visit [GitHub Community](https://github.community/)

---

*Last updated: October 2025*
