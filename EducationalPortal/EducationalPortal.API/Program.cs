using EducationalPortal.API.Config;
using EducationalPortal.API.DI;
using EducationalPortal.Infrastructure;
using EducationalPortal.Infrastructure.DataInitializer;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenServices(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabRouteTransformer()));
}).AddNewtonsoftJson(options => 
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowMyOrigin",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("X-Pagination");
    });
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddFluentValidators();
builder.Logging.AddLogger(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<PasswordHasher>>();
await DbInitializer.Initialize(context, logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseRouting();

app.UseCors("allowMyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
