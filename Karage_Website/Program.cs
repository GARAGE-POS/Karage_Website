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
    app.UseHsts(); // Enables HSTS (Strict Transport Security)
}

// Use static files, routing, and authorization
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ✅ Defines the main route pattern for all controllers
//app.MapControllerRoute(
//    name: "localized",
//    pattern: "{lang=en}/{controller=Home}/{action=Index}/{id?}",
//    defaults: new { lang = "en" },
//    constraints: new { lang = "en|ar" } // Ensures only 'en' or 'ar' are allowed
//);
app.MapControllerRoute(
    name: "localized",
    pattern: "{lang=en}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index", lang = "en" },
    constraints: new { lang = "en|ar" }
);

app.MapControllerRoute(
    name: "custom_routes",
    pattern: "{lang:regex(^en|ar$)}/{action}",
    defaults: new { controller = "LandingPages", action = "Index" }
);

app.MapControllerRoute(
    name: "custom_routes",
    pattern: "{lang:regex(^en|ar$)}/{action}",
    defaults: new { controller = "MarketPlace", action = "Index" }
);

app.MapControllerRoute(
    name: "custom_routes",
    pattern: "{lang:regex(^en|ar$)}/{action}",
    defaults: new { controller = "BusinessType", action = "Index" }
);



// Redirect root "/" to "/en/home"
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
