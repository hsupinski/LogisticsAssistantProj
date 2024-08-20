using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

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
