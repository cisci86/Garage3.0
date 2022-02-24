using Garage_2._0.Automapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Garage_2._0.Services;
using Garage_2._0.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GarageVehicleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GarageVehicleContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVehicleTypeSelectListService, VehicleTypeSelectListService>();

builder.Services.AddAutoMapper(typeof(GarageMappings));

//https://gist.github.com/AndreasAmMueller/38c1a8d76ecd4450b4f75a479f3293c1 To make localisation work, because Microsoft hates non US citizens...
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new CustomFloatingPointModelBinderProvider());
});

var app = builder.Build();

app.SeedDataAsync().GetAwaiter().GetResult();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehicles}/{action=VehiclesOverview}/{id?}");

app.Run();
