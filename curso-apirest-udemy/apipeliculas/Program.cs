using System.Text;
using apipeliculas;
using apipeliculas.src.Application.Services;
using apipeliculas.src.Data;
using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Domain.Models;
using apipeliculas.src.Infraestructure.Repositories;
using apipeliculas.src.Mapper;
using apipeliculas.src.Repositories;
using apipeliculas.src.Repositories.Impl;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURACIÓN BASE
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret_Key");

// DATABASE
builder.Services.AddDbContext<AplicationDbContext>(opciones =>
    opciones.UseNpgsql(builder.Configuration.GetConnectionString("cnnPostgres"))
);

// IDENTITY
//soporte para autenticacion con .NET identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AplicationDbContext>();

// REPOSITORIES
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// APPLICATION SERVICES
builder.Services.AddScoped<IImageStoreService, CloudinaryService>();

// CONTROLLERS & CACHE
builder.Services.AddControllers(option =>
{
    //Perfil de cache global
    option.CacheProfiles.Add("CachePorDefault30",
        new CacheProfile() { Duration = 30 });
});

// AUTOMAPPER
builder.Services.AddAutoMapper(cfg => { }, typeof(MoviesMapper));

// AUTHENTICATION (JWT)
//condigurar authenticacion
builder.Services.AddAuthentication(
    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
    x =>
    {
        x.RequireHttpsMetadata = false; //desactivado SSL
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(key)
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

// SWAGGER
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "Autenticaci�n JWT usando el esquema Bearer. \r\n\r\n " +
            "Ingresa la palabra 'Bearer' seguido de un [espacio] y despu�s su token en el campo de abajo.\r\n\r\n" +
            "Ejemplo: \"Bearer tkljk125jhhk\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Peliculas Api V1",
        Description = "Api de Peliculas Versi�n 1",
        TermsOfService = new Uri("https://render2web.com/promociones"),
        Contact = new OpenApiContact
        {
            Name = "render2web",
            Url = new Uri("https://render2web.com/promociones")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            Url = new Uri("https://render2web.com/promociones")
        }
    }
    );
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Peliculas Api V2",
        Description = "Api de Peliculas Versi�n 2",
        TermsOfService = new Uri("https://render2web.com/promociones"),
        Contact = new OpenApiContact
        {
            Name = "render2web",
            Url = new Uri("https://render2web.com/promociones")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            Url = new Uri("https://render2web.com/promociones")
        }
    }
    );
    options.OperationFilter<FileUploadOperationFilter>();
});

// CORS
builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// API VERSIONING
var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true; //asume la version
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    option.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version")
    );
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});

// BUILD APP
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// HTTP PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculasV2");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("PoliticaCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// LOGS
app.Lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("Swagger UI disponible en: http://localhost:5000/swagger");
    logger.LogInformation("index.html en: http://localhost:5000/index.html");
});

app.Run();
