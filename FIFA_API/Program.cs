using FIFA_API.Contracts;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;
using FIFA_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

// Managers
builder.Services.AddScoped<ILangueManager, LangueManager>(); 
builder.Services.AddScoped<IPaysManager, PaysManager>();
builder.Services.AddScoped<IUtilisateurManager, UtilisateurManager>();
builder.Services.AddScoped<ICategorieProduitManager, CategorieProduitManager>();
builder.Services.AddScoped<ICouleurManager, CouleurManager>();
builder.Services.AddScoped<ITailleProduitManager, TailleProduitManager>();
builder.Services.AddScoped<ICompetitionManager, CompetitionManager>();
builder.Services.AddScoped<IGenreManager, GenreManager>();
builder.Services.AddScoped<INationManager, NationManager>();
builder.Services.AddScoped<IProduitManager, ProduitManager>();
builder.Services.AddScoped<IThemeVoteManager, ThemeVoteManager>();

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
builder.Services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>(
    _ => new Argon2PasswordHasher(secret: Convert.FromHexString(builder.Configuration["Argon2:Secret"]))
);

// Filters

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
