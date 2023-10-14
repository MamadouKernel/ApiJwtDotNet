using boka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

// Crée un objet de construction de l'application Web.
var builder = WebApplication.CreateBuilder(args);

// Configuration : Ajoute un fichier de configuration JSON.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Ajoute des services au conteneur de dépendances.
builder.Services.AddControllers(); // Ajoute le support des contrôleurs.
builder.Services.AddEndpointsApiExplorer(); // Ajoute un explorateur d'API.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtApi", Version = "v1" });
    // Configuration Swagger pour la documentation de l'API.
});

// Configuration de l'authentification JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddTransient<IJwtAuthService, JwtAuthService>();
// Ajoute un service pour l'authentification JWT.

var app = builder.Build();
// Crée l'application à partir de la configuration précédente.

// Configure le pipeline de traitement des requêtes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Active Swagger en environnement de développement.
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redirige les requêtes HTTP vers HTTPS.
app.UseRouting(); // Gère la gestion des routes.
app.UseAuthentication(); // Active l'authentification.
app.UseAuthorization(); // Active l'autorisation d'accès aux ressources.

app.MapControllers(); // Mappe les contrôleurs.

app.Run();
