global using Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;


builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = null;
    options.MaxSendMessageSize = null;
});

builder.Services.AddLocalization();

builder.Services.AddRazorPages(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    o.Conventions.AuthorizeFolder("/");
    //o.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminRoleRequired"); 

    o.Conventions.AllowAnonymousToFolder("/Identity/");
}).AddRazorRuntimeCompilation();

builder.Services.AddResponseCaching();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(IdentityChannelClient));
builder.Services.AddSingleton(typeof(CoreChannelClient));
builder.Services.AddScoped(typeof(ViewRenderService));


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.Cookie.Name = "Currency";
        options.AccessDeniedPath = "/Identity/AccessDenied";

        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);


        options.SlidingExpiration = true;
        options.LoginPath = "/Identity/Login";
        options.LogoutPath = "/Identity/Logout";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoginRequired", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminRoleRequired", policy => policy.RequireRole("Admin"));
});

// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});






































var app = builder.Build();

app.UseResponseCaching();
// Configure the HTTP request pipeline.
app.UseCustomExceptionHandler(app.Environment);


app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();




app.Run();