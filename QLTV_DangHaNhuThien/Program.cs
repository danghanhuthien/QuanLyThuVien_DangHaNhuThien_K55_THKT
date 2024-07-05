using QLTV_DangHaNhuThien.Components.Data;
using QLTV_DangHaNhuThien.Components.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using QLTV_DangHaNhuThien.Components.UseDapper;
using Dapper;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// setup dapper
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'Default' is missing or empty.");
}
builder.Services.AddScoped<IConnectDapper>(sp => new ConnectDapper(connectionString));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// t?t ?i c?nh báo ch?ng gi? m?o
builder.Services.AddControllersWithViews(options =>
    options.Filters.Remove(new AutoValidateAntiforgeryTokenAttribute()));

//login Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/";
        options.AccessDeniedPath = "/denied";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(120);
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();



//BookDbcontext
builder.Services.AddDbContext<BookDbcontext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//service User
builder.Services.AddScoped<IUserService, UserService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<QLTV_DangHaNhuThien.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
