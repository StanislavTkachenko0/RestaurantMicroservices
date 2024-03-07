using Duende.IdentityServer.Services;
using Mango.Services.Identity;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Models;
using Mango.Services.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var identityBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(SD.IdentityResources)
    .AddInMemoryApiScopes(SD.ApiScopes)
    .AddInMemoryClients(SD.Clients)
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Получаем сервис из области
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();

    // Вызываем метод Initialize
    dbInitializer.Initialize();

    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
