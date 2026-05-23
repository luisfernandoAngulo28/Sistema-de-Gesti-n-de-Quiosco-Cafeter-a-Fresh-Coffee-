using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuioscoAPI.Data;
using QuioscoAPI.Mapping;
using QuioscoAPI.Middleware;
using QuioscoAPI.Repositories;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services;
using QuioscoAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL + EF Core
builder.Services.AddDbContext<QuioscoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfileLFAH));

// Repositories
builder.Services.AddScoped<ICategoriaRepositoryLFAH, CategoriaRepositoryLFAH>();
builder.Services.AddScoped<IProductoRepositoryLFAH, ProductoRepositoryLFAH>();
builder.Services.AddScoped<IPedidoRepositoryLFAH, PedidoRepositoryLFAH>();
builder.Services.AddScoped<IUserRepositoryLFAH, UserRepositoryLFAH>();
builder.Services.AddScoped<IMesaRepositoryLFAH, MesaRepositoryLFAH>();
builder.Services.AddScoped<IPagoRepositoryLFAH, PagoRepositoryLFAH>();

// Services
builder.Services.AddScoped<ICategoriaServiceLFAH, CategoriaServiceLFAH>();
builder.Services.AddScoped<IProductoServiceLFAH, ProductoServiceLFAH>();
builder.Services.AddScoped<IPedidoServiceLFAH, PedidoServiceLFAH>();
builder.Services.AddScoped<IUserServiceLFAH, UserServiceLFAH>();
builder.Services.AddScoped<IJwtServiceLFAH, JwtServiceLFAH>();
builder.Services.AddScoped<IMesaServiceLFAH, MesaServiceLFAH>();
builder.Services.AddScoped<IPagoServiceLFAH, PagoServiceLFAH>();

// JWT Auth
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// JSON: snake_case para compatibilidad con React/Laravel
// Validación automática de DTOs retorna 422 con errores en snake_case
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
                );
            return new UnprocessableEntityObjectResult(new { errors });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Fresh Coffee — Quiosco API",
        Version = "v1",
        Description = """
            API REST para el Sistema de Gestión de Quiosco Cafetería.
            Desarrollada con ASP.NET Core 8, Entity Framework Core y PostgreSQL.
            Aplica arquitectura N-Capas, principios SOLID y AutoMapper.

            **Proyecto Final — Desarrollo Backend con .NET y C# Nivel Intermedio**
            Estudiante: Luis Fernando Angulo Heredia (LFAH)
            Docente: Lic. Ing. Limber Mamani Canaza
            Institución: Posgrado UPEA
            """,
        Contact = new()
        {
            Name = "Luis Fernando Angulo Heredia",
            Email = "fernando.fa671@gmail.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT obtenido desde **POST /api/auth/login**.\n\nEjemplo: `Bearer eyJhbGci...`"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((_, _) => true);
});

// CORS: permite peticiones del React (localhost:5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");
app.UseMiddleware<ExceptionMiddlewareLFAH>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed inicial de categorías y productos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuioscoDbContext>();
    await DataSeederLFAH.SeedAsync(context);
}

app.Run();
