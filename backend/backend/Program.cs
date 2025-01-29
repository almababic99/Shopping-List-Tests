using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Services;
using Application.Interfaces;
using Application.Config;
using Hangfire;
using Application.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShoppingListDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));   // This is registering the ShoppingListDbContext
builder.Services.AddScoped<IShopperRepository, ShopperRepository>();  // This registers the ShopperRepository as a scoped service that implements the IShopperRepository interface.
builder.Services.AddScoped<IShopperService, ShopperService>();        // This registers the ShopperService as a scoped service that implements the IShopperService interface.
builder.Services.AddScoped<IItemRepository, ItemRepository>();        // This registers the ItemRepository as a scoped service that implements the IItemRepository interface.
builder.Services.AddScoped<IItemService, ItemService>();              // This registers the ItemService as a scoped service that implements the IItemService interface.
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();   // This registers the ShoppingListRepository as a scoped service that implements the IShoppingListRepository interface.
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();         // This registers the ShoppingListService as a scoped service that implements the IShoppingListService interface.

builder.Services.AddMediatR(cf => cf.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly));

builder.Services.AddControllers();

builder.Services.AddHangfire(configuration =>
    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<ItemQuantityUpdaterJob>();
builder.Services.AddScoped<NewShoppingListAddedInfoJob>();
builder.Services.AddScoped<WelcomeMessageJob>();

builder.Services.AddCors(options =>    // Adding Cors to the application
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")    // Allow requests from Angular frontend
              .AllowAnyMethod()                       // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader();                     // Allow all headers
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowLocalhost");    // Applying added Cors to the application

app.UseHangfireDashboard();
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<ItemQuantityUpdaterJob>("item-quantity-updater", job => job.Execute(), Cron.Minutely);    // Recurring job that runs every minute 

BackgroundJob.Schedule<WelcomeMessageJob>(job => job.Execute(), TimeSpan.FromSeconds(10));   // Delayed job delayed by 10 seconds

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
