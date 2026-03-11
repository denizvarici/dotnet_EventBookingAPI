using EventBooking.Application.Interfaces;
using EventBooking.Application.Mappings;
using EventBooking.Application.Services;
using EventBooking.Application.Validators;
using EventBooking.Domain.Interfaces;
using EventBooking.Infrastructure.Repositories;
using Security;
using FluentValidation;
using EventBooking.API.Middlewares;
using EventBooking.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//DB configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

//DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IValidationService, ValidationService>();

//Libraries DI
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = DataHolder.AutoMapperLicenceKey;
    cfg.AddProfile<GeneralMapping>();
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();





var app = builder.Build();
//middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();






app.Run();
