using EducationalPortal.API;
using EducationalPortal.Infrastructure;
using EducationalPortal.Infrastructure.DataInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenAuthentication(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.ConfigureCORS();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddFluentValidators();
builder.Logging.AddLogger(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await DbInitializer.InitializeDb(app);
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
