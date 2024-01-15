using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using ICAP_ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

const string secretPath = "/mnt/secrets-store";
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

builder.Services.UseAzureServiceBusPublisher(builder.Configuration);
builder.Services.UseAzureServiceBusHandler(builder.Configuration);

builder.Services.AddMongo()
    .AddMongoRepository<FriendRequest>("friendrequests")
    .AddMongoRepository<Friends>("friendlists");

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
