using EventBooking.API.Middlewares;
using EventBooking.API.Transformers;
using EventBooking.Application.Interfaces;
using EventBooking.Application.Mappings;
using EventBooking.Application.Services;
using EventBooking.Application.Validators;
using EventBooking.Domain.Entities;
using EventBooking.Domain.Interfaces;
using EventBooking.Infrastructure.BackgroundServices;
using EventBooking.Infrastructure.Extensions;
using EventBooking.Infrastructure.Identity;
using EventBooking.Infrastructure.Persistence;
using EventBooking.Infrastructure.Repositories;
using EventBooking.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Security;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

//DB configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

//DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//caching DI
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379";
        options.InstanceName = "EventBooking_";
    });
    builder.Services.AddScoped<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ICacheService, InMemoryCacheService>();
}

//caching DI (Redis)


//Libraries DI
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = DataHolder.AutoMapperLicenceKey;
    cfg.AddProfile<GeneralMapping>();
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();

//EF Core Identity Registery
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//JWT System Registry
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});
//openapi configuration
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
//redis configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "EventBooking_";
});
//background worker registry
builder.Services.AddHostedService<EventStatusWorker>();


var app = builder.Build();
//middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

//runtime seeds
await app.SeedIdentityDataAsync(); //seeds default users on startup


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("EventBooking API")
            .AddPreferredSecuritySchemes("Bearer")
            .AddHttpAuthentication("Bearer", bearer =>
            {
                bearer.Token = "";
            });
    });

}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();






app.Run();
