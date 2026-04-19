using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using StockManager.API.Data;
using StockManager.API.Interfaces.CatalogInterfaces;
using StockManager.API.Middlewares;
using StockManager.API.Services.CatalogServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// DbContext
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//Cloudinary setup
var cloudinaryUrl = builder.Configuration["CloudinarySettings:Url"];

Cloudinary? cloudinary = null;

if (!string.IsNullOrWhiteSpace(cloudinaryUrl) &&
    cloudinaryUrl.StartsWith("cloudinary://"))
{
    cloudinary = new Cloudinary(cloudinaryUrl);
}

if (cloudinary != null)
{
    builder.Services.AddSingleton(cloudinary);
}

//Adding CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://stock-manager-vercel.vercel.app",   
                "http://localhost:5173"          
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});




// Services
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton(new Cloudinary(cloudinaryUrl));


builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseMiddleware<TraceMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
