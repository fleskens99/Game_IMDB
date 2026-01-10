using Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repo;
using Repos;
using Services; 

var builder = WebApplication.CreateBuilder(args);

// Register services BEFORE Build
builder.Services.AddRazorPages();
builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<IAddGameService, AddGameService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRatingRepo, RatingRepo>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware ordering
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
