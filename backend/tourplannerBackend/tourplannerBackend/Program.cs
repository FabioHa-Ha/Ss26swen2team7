using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using tourplannerBackend.Filters;
using tourplannerBackend.Middleware;
using tourplannerBackend.Repositories;
using tourplannerBackend.Services;
using tourPlannerBackend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Repositories
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITourLogRepository, TourLogRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransportTypeRepository, TransportTypeRepository>();
builder.Services.AddScoped<IDifficultyRepository, DifficultyRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Services
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourLogService, TourLogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImportExportService, ImportExportService>();
// Singleton: in-memory collection must survive across requests
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddHttpClient<IRouteService, RouteService>();

// --- Error Handling (Task 5) --------------------------------------------------
// 1. RFC-7807 ProblemDetails: standard error response shape for the whole API.
//    [ApiController] automatically uses this for model-validation errors (400).
builder.Services.AddProblemDetails();

// 2. Global Exception Handler: catches all unhandled exceptions and maps them to
//    ProblemDetails. Registered here; applied via app.UseExceptionHandler() below.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 3. DomainExceptionFilter: applied selectively via [TypeFilter] on TourController.
//    Must be in DI so TypeFilter can resolve it with constructor injection.
builder.Services.AddScoped<DomainExceptionFilter>();
// -----------------------------------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// UseExceptionHandler() must be placed early in the pipeline (before routing and auth)
// so it can intercept exceptions from any subsequent middleware or action.
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
