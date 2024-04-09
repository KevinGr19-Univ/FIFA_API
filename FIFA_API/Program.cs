using FIFA_API.Contracts;
using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Repositories;
using FIFA_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Twilio;

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

builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();

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
if (!string.IsNullOrEmpty(twilio) && bool.Parse(twilio))
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
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#region Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUnitOfWorkCommande, UnitOfWorkCommande>();
builder.Services.AddScoped<IUnitOfWorkJoueur, UnitOfWorkJoueur>();
builder.Services.AddScoped<IUnitOfWorkProduit, UnitOfWorkProduit>();
builder.Services.AddScoped<IUnitOfWorkPublication, UnitOfWorkPublication>();
builder.Services.AddScoped<IUnitOfWorkVote, UnitOfWorkVote>();
builder.Services.AddScoped<IUnitOfWorkUserServices, UnitOfWorkUserServices>();

builder.Services.AddScoped<IManagerAlbum, ManagerAlbum>();
builder.Services.AddScoped<IManagerArticle, ManagerArticle>();
builder.Services.AddScoped<IManagerAuth2FALogin, ManagerAuth2FALogin>();
builder.Services.AddScoped<IManagerAuthEmailVerif, ManagerAuthEmailVerif>();
builder.Services.AddScoped<IManagerAuthPasswordReset, ManagerAuthPasswordReset>();
builder.Services.AddScoped<IManagerBlog, ManagerBlog>();
builder.Services.AddScoped<IManagerCategorieProduit, ManagerCategorieProduit>();
builder.Services.AddScoped<IManagerClub, ManagerClub>();
builder.Services.AddScoped<IManagerCommande, ManagerCommande>();
builder.Services.AddScoped<IManagerCommentaireBlog, ManagerCommentaireBlog>();
builder.Services.AddScoped<IManagerCompetition, ManagerCompetition>();
builder.Services.AddScoped<IManagerCouleur, ManagerCouleur>();
builder.Services.AddScoped<IManagerDocument, ManagerDocument>();
builder.Services.AddScoped<IManagerFaqJoueur, ManagerFaqJoueur>();
builder.Services.AddScoped<IManagerGenre, ManagerGenre>();
builder.Services.AddScoped<IManagerJoueur, ManagerJoueur>();
builder.Services.AddScoped<IManagerLangue, ManagerLangue>();
builder.Services.AddScoped<IManagerLigneCommande, ManagerLigneCommande>();
builder.Services.AddScoped<IManagerNation, ManagerNation>();
builder.Services.AddScoped<IManagerPays, ManagerPays>();
builder.Services.AddScoped<IManagerPhoto, ManagerPhoto>();
builder.Services.AddScoped<IManagerProduit, ManagerProduit>();
builder.Services.AddScoped<IManagerPublication, ManagerPublication>();
builder.Services.AddScoped<IManagerStatistiques, ManagerStatistiques>();
builder.Services.AddScoped<IManagerStatusCommande, ManagerStatusCommande>();
builder.Services.AddScoped<IManagerStockProduit, ManagerStockProduit>();
builder.Services.AddScoped<IManagerTailleProduit, ManagerTailleProduit>();
builder.Services.AddScoped<IManagerThemeVote, ManagerThemeVote>();
builder.Services.AddScoped<IManagerThemeVoteJoueur, ManagerThemeVoteJoueur>();
builder.Services.AddScoped<IManagerTrophee, ManagerTrophee>();
builder.Services.AddScoped<IManagerTypeLivraison, ManagerTypeLivraison>();
builder.Services.AddScoped<IManagerUtilisateur, ManagerUtilisateur>();
builder.Services.AddScoped<IManagerVarianteCouleurProduit, ManagerVarianteCouleurProduit>();
builder.Services.AddScoped<IManagerVideo, ManagerVideo>();
builder.Services.AddScoped<IManagerVoteUtilisateur, ManagerVoteUtilisateur>();
#endregion

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
