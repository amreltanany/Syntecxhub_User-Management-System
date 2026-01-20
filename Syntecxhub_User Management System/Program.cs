using MongoDB.Driver;
using Syntecxhub_User_Management_System.Repositories;
using Microsoft.OpenApi.Models; // Required for OpenApiInfo

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- SWAGGER CONFIGURATION (Services) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Syntecx User Management API",
        Version = "v1"
    });
});

// --- MONGODB CONFIGURATION ---
var connectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClient = new MongoClient(connectionString);

builder.Services.AddSingleton<IMongoClient>(mongoClient);

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    // Make sure "SyntecxDb" matches your database name in MongoDB Compass
    return client.GetDatabase("SyntecxDb");
});

// --- REPOSITORY REGISTRATION ---
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// --- SWAGGER MIDDLEWARE ---
// Only enable in development for security
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Fixes the error in your third screenshot
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Syntecx API V1");
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();