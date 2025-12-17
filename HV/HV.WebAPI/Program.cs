using FluentValidation;
using HV.BLL.Services;
using HV.BLL.Services.Abstractions;
using HV.BLL.Validators;
using HV.DAL;
using HV.DAL.Abstractions;
using HV.WebAPI.ActionFilters;
using HV.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ensure wwwroot directory exists
var wwwrootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (!Directory.Exists(wwwrootPath))
    Directory.CreateDirectory(wwwrootPath);

builder.Services.AddControllers(options => { options.Filters.Add<CustomExceptionFilterAttribute>(); });

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository and UnitOfWork
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Business Logic Services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ILandmarkTagService, LandmarkTagService>();
builder.Services.AddScoped<ILandmarkService, LandmarkService>();

// Infrastructure Services
builder.Services.AddScoped<HV.BLL.Services.Abstractions.IFileStorageService, HV.WebAPI.Services.FileStorageService>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCountryRequestValidator>();

var app = builder.Build();

// Apply migrations to db
await app.MigrateDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "HeritageVault API v1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});

app.MapControllers();

app.Run();