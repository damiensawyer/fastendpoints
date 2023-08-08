using System.Text.Json;

using fastendpointsDemo.Extensions;

using FastEndpoints.Swagger;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

logger.Information("Starting up");

try
{

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.AllowSynchronousIO = false;
});

 if (builder.Environment.IsProduction())
    {
        builder.Logging.ClearProviders();
    }
    builder.Services.AddSingleton(logger);

builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes = new Type[] { };
});

builder.Services.AddSwaggerDoc(addJWTBearerAuth: false);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
    options.Errors.ResponseBuilder = (errors, _, _) => errors.ToResponse();
    options.Errors.StatusCode = StatusCodes.Status422UnprocessableEntity;
    options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(x => x.ConfigureDefaults());
}

await app.RunAsync();

}
catch (Exception ex)
{
    logger.Fatal(ex, "Unhandled exception");
}
finally
{
    logger.Error("Shut down complete");
    Log.CloseAndFlush();
}
public partial class Program
{
}