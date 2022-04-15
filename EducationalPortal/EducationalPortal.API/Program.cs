using EducationalPortal.API.DI;
using EducationalPortal.Infrastructure.DataInitializer;
using EducationalPortal.Infrastructure.DI;
using EducationalPortal.Infrastructure.EF;
using Newtonsoft.Json;

var context = new ApplicationContext();
await DbInitializer.Initialize(context);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenServices(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options => 
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
builder.Services.AddInfrastructure();
builder.Services.AddServices();
builder.Services.AddFluentValidators();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("allowMyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
