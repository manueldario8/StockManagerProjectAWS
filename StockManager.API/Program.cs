using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockManager.API.Data;
using StockManager.API.Entities.Models.Users;
using StockManager.API.Interfaces.AuthInterfaces;
using StockManager.API.Interfaces.CatalogInterfaces;
using StockManager.API.Middlewares;
using StockManager.API.Services.AuthServices;
using StockManager.API.Services.CatalogServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");

// Add services to the container.
// DbContext
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        )
    };
});

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

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();


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
