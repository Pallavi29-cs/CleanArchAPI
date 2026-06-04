using System.Text;
using CleanArchAPI.API.Middleware;
using CleanArchAPI.Data;
using CleanArchAPI.Data.Abstractions;
using CleanArchAPI.Data.Implementations;
using CleanArchAPI.Service.Abstractions;
using CleanArchAPI.Service.Implementations;
using CleanArchAPI.Store.Abstraction;
using CleanArchAPI.Store.Implemenation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// ============================================================
// Serilog Logging Setup
// ============================================================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/cleanarchapi-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers();

// ============================================================
// Dependency Injection
// Flow: Controller → Service → Store → Repository → Database
// ============================================================

// Data Layer
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Store Layer
builder.Services.AddScoped<IUserStore, UserStore>();
builder.Services.AddScoped<IProductStore, ProductStore>();
builder.Services.AddScoped<IOrderStore, OrderStore>();

// Service Layer
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ============================================================
// JWT Authentication
// ============================================================
var jwt = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwt["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwt["Issuer"],
        ValidAudience            = jwt["Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew                = TimeSpan.Zero
    };
});

// Role Based Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",  p => p.RequireRole("Admin"));
    options.AddPolicy("ManagerUp", p => p.RequireRole("Admin", "Manager"));
    options.AddPolicy("AllUsers",  p => p.RequireRole("Admin", "Manager", "User"));
});

// ============================================================
// Swagger with JWT Support
// ============================================================
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CleanArch API",
        Version = "v1",
        Description = "ASP.NET Core Web API — Clean Architecture with JWT, Role-Based Auth, Pagination, Bulk Upload"
    });



    // ================================
    // XML Comments For API Summary
    // ================================
    var xmlFile =
        $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";


    var xmlPath =
        Path.Combine(
            AppContext.BaseDirectory,
            xmlFile);


    c.IncludeXmlComments(xmlPath);




    // ================================
    // JWT Swagger Authorization
    // ================================
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });





    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =
                new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },


            Array.Empty<string>()
        }
    });

});

// ============================================================
// Build and Configure Middleware Pipeline
// ============================================================
var app = builder.Build();

// Global Exception Handler — must be FIRST
app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArch API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Log.Information("CleanArch API started on {Env}", app.Environment.EnvironmentName);
app.Run();
