using Domain;
using EmailVerifyService.Business;
using EmailVerifyService.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.OpenApi;
using OperationManagementService.Security;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options => options.OperationFilter<AuthenticationKeyHeader>());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EmailVerifyService",
        Version = "v1",
        Description = "API para verificación de correos"
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<CustomResponseCreatedExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CustomResponseBadRequestExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CustomResponseOKExample>();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CustomAuthenticationHandler.SchemaName)
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(
        CustomAuthenticationHandler.SchemaName, options => {
        });

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(0, 1);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

builder.Services.AddScoped<FunctionalityBaseController>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<VerfyCodeFunctionality>();
builder.Services.AddScoped<ServiceBaseFunctionality>();
builder.Services.AddScoped<RazorViewToStringRenderer>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<RateLimitingOptions>(
    builder.Configuration.GetSection("RateLimiting"));

// Rate limiting configuration
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("IpPolicy", context =>
    {
        // Get configuration from appsettings
        var config = builder.Configuration.GetSection("RateLimiting").Get<RateLimitingOptions>();
        // Use the IP address as the partition key
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        // Allow a maximum of 5 request per minute per IP address
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = config.PermitLimit,
                Window = TimeSpan.FromSeconds(config.WindowSeconds),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    // Custom response for rate limited requests
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.Headers["Retry-After"] = "60"; // clients should retry after 60 seconds
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = StatusCodes.Status429TooManyRequests,
            Message = "Too many requests. Please try again later."
        }, cancellationToken: token);
    };
});

var app = builder.Build();

// redirect root to swagger
app.Use(async (context, next) => {
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailVerifyService v1"); });
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
