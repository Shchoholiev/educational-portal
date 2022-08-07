using EducationalPortal.API;
using EducationalPortal.Infrastructure;
using Microsoft.AspNetCore.SpaServices.AngularCli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenAuthentication(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.ConfigureCORS();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddFluentValidators();
builder.Logging.AddLogger(builder.Configuration);
builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "ClientApp/dist";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    spa.UseAngularCliServer(npmScript: "start");
});

app.Run();