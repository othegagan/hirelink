using hirelink.DbContext;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<HirelinkDbContext, HirelinkDbContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => {
    option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
    option.LoginPath = "/login";
    option.AccessDeniedPath = "/login";
});

builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseExceptionHandler("/Error"); // Redirect to the Error controller's Index action
app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Redirect to the Error controller's Index action with the HTTP status code

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    // Handle errors
    app.UseExceptionHandler("/Error"); // Redirect to the Error controller's Index action
    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Redirect to the Error controller's Index action with the HTTP status code
    app.UseHsts();
} else {
    app.UseDeveloperExceptionPage();
}

app.Use(async (context, next) => {

    // Set cache control headers to prevent caching
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";

    await next.Invoke();
});



app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Jobs}/{action=Jobs}/{id?}");

app.Run();
