using FIFA_API.Contracts;
using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

var builder = WebApplication.CreateBuilder(args);

// Logging
if (builder.Environment.IsProduction())
{
    builder.Services.Configure<AzureFileLoggerOptions>(options =>
    {
        options.FileName = "logs-";
        options.FileSizeLimit = 50 * 1024;
        options.RetainedFileCountLimit = 5;
    });

    builder.Logging.AddAzureWebAppDiagnostics(options =>
    {
        options.BlobName = "logs.txt";
    });
}

// Env variables
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables(prefix: "FIFA_");

// DB & Controllers
builder.Services.AddDbContext<FifaDbContext>();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(x => {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    doc =>
    {
        var xmlFile = Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml");
        doc.IncludeXmlComments(xmlFile);
    }
);

// Authentication
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.User, Policies.UserPolicy());
    config.AddPolicy(Policies.DirecteurVente, Policies.DirecteurVentePolicy());
    config.AddPolicy(Policies.ServiceCommande, Policies.ServiceCommandePolicy());
    config.AddPolicy(Policies.ServiceExpedition, Policies.ServiceExpeditionPolicy());
    config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
});

// Services
Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddSingleton<IEmailSender, EmailService>();
builder.Services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>(
    _ => new Argon2PasswordHasher(secret: Convert.FromHexString(builder.Configuration["Argon2:Secret"]))
);

builder.Services.AddScoped<IEmailVerificator, EmailVerificator>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<ILogin2FAService, Login2FAService>();

var twilio = builder.Configuration["Twilio:Enabled"];
if (string.IsNullOrEmpty(twilio) || bool.Parse(twilio))
{
    TwilioClient.Init(
        builder.Configuration["Twilio:AccountSid"],
        builder.Configuration["Twilio:AuthToken"]);

    builder.Services.AddScoped<ISmsService, TwilioSmsService>(
        svp => new TwilioSmsService(
            builder.Configuration["Twilio:PhoneNumber"],
            builder.Configuration["Twilio:OverrideTo"],
            svp.GetService<ILogger<ISmsService>>())
    );
}
else
{
    builder.Services.AddScoped<ISmsService, DebugSmsService>();
}

// CORS
var policyName = "FIFA_CORS";
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors(policyName);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
