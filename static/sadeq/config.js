// Configuration for the application
const CONFIG = {
    // API Base URL - change this to your production API endpoint when deploying
    API_BASE_URL: window.location.hostname === 'localhost' 
        ? 'http://localhost:7071/api'
        : 'https://functions-uat.karage.co/api', // Update this with your actual production API URL
    
    // Application settings
    APP_NAME: 'نظام إدارة العقود الرقمية',
    DEFAULT_TEMPLATE_ID: '55869aa7-ca2c-4d88-b63e-6bc8ee2036ed'
};

// Make CONFIG globally available
window.CONFIG = CONFIG;

// Add a small debug log to confirm the script is loaded
console.log('CONFIG loaded successfully:', CONFIG);
