using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Repositories;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("NLog configured correctly");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var assistantDbContextConnectionString = builder.Configuration.GetConnectionString("AssistantDbContextConnection");
    var authDbContextConnectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection");

    builder.Services.AddDbContext<AssistantDbContext>(options =>
        options.UseSqlServer(assistantDbContextConnectionString));

    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlServer(authDbContextConnectionString));


    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AuthDbContext>();

    builder.Services.AddScoped<ITruckRepository, TruckRepository>();
    builder.Services.AddScoped<ITransitRepository, TransitRepository>();
    builder.Services.AddScoped<ITruckService, TruckService>();
    builder.Services.AddScoped<ITransitService, TransitService>();
    builder.Services.AddScoped<IAccountService, AccountService>();

    var app = builder.Build();

    using (var Scope = app.Services.CreateScope())
    {
        var assistantContext = Scope.ServiceProvider.GetRequiredService<AssistantDbContext>();
        assistantContext.Database.Migrate();

        var authContext = Scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        authContext.Database.Migrate();
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

