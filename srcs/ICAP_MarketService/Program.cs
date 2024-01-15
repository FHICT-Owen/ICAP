using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

const string secretPath = "/mnt/secrets-store";
try
{
    var secretFiles = Directory.GetFiles(secretPath);
    if (secretFiles.Any())
    {
        foreach (var file in secretFiles)
        {
            var secretName = Path.GetFileName(file);
            var secretValue = File.ReadAllText(file);
            Environment.SetEnvironmentVariable(secretName, secretValue);
        }
    }
}
catch (Exception) { }

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedSpecificOrigins", builder =>
        builder.WithOrigins("http://localhost",
                            "https://localhost",
                            "https://icap.odb-tech.com")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBus"]);
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowedSpecificOrigins");

app.Run();
