var builder = WebApplication.CreateBuilder(args);
// builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
// builder.Configuration.AddEnvironmentVariables();

// Application would rely on configmap for all configurations

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api", () => "This is Product API, hosted in AKS.");

app.Run();
