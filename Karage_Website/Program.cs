using Karage_Website.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // Consider enabling HTTPS redirection in Production
    app.UseHsts(); // Adds HTTP Strict Transport Security (HSTS) headers
}

// Use static files, routing, and authorization
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "localized",
    pattern: "{lang=en}/{action=Index}/{id?}", // Removes the controller from URL
    defaults: new { controller = "Home", action = "Index", lang = "en" },
    constraints: new { lang = "en|ar" } // Supports only 'en' and 'ar'
);

// Redirect root "/" to default language
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/en/home");
    }
    else
    {
        await next();
    }
});


app.Run();
