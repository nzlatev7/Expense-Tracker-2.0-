using Expense_Tracker_2._0;
using Expense_Tracker_2._0.Hubs;
using Expense_Tracker_2._0.Services;
using Expense_Tracker_2._0.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//connectionString
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Singleton);

//jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
});

//SingalR
builder.Services.AddSignalR();

//configure the CORS service
builder.Services.AddCors();

//Services
//this add out serivce to the dependency injection system
//take care of creating and managing the service instances
builder.Services.AddScoped<IJwtService, JwtService>(); //register the JwtService
builder.Services.AddTransient<IEmailService, EmailService>(); // Register the Email service
builder.Services.AddTransient<IValidationToken, ValidationTokenService>(); // Register the ValidationToken service
builder.Services.AddTransient<ICloudService, CloudService>(); // Register the ValidationToken service

//Background service
builder.Services.AddHostedService<ExpiredTokensCleanupService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
builder.WithOrigins("http://localhost:4200").AllowCredentials() // Allow requests from a specific origin
// It is generally recommended to specify the allowed origins explicitly
// rather than allowing requests from any origin (*)
.AllowAnyHeader()
.AllowAnyMethod()); // Get, Post, Put Delete

//jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//mapHub SingalR
app.MapHub<ChatHub>("/chatHub");

app.Run();
