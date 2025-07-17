using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Implementations;
using SolYSalEcommerce.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// --- Configuración de Swagger/OpenAPI con soporte JWT ---
builder.Services.AddSwaggerGen(option =>
{
    // Define el esquema de seguridad para JWT (Bearer Token)
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, introduce un token JWT válido (ejemplo: 'Bearer TU_TOKEN')",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Asegura que las operaciones de API requieran un token Bearer para autenticación
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Debe coincidir con el nombre definido arriba
                }
            },
            new string[] { }
        }
    });
});
// --- Fin de Configuración de Swagger/OpenAPI ---


// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<IdentityRole<Guid>>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
});

// --- Configuración de CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            // Permite solicitudes desde tu frontend local.
            // Es CRÍTICO que la URL (incluyendo protocolo y puerto) coincida exactamente con la de tu frontend.
            policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "http://localhost:8080") // <-- ¡ACTUALIZADO AQUÍ!
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // .AllowCredentials(); // Descomenta si tu frontend envía cookies o necesita credenciales con la solicitud
        });
});
// --- Fin de Configuración de CORS ---


// Register your application services here
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Si planeas agregar servicios de pago como Wompi, los registrarías aquí:
// builder.Services.AddScoped<IPaymentService, WompiPaymentService>(); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(); // Habilita la política CORS por defecto

// Habilita el servicio de archivos estáticos (imágenes, CSS, JS, etc.)
// Asume que tus archivos estáticos están en la carpeta 'wwwroot' del proyecto.
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- Bloque para la siembra de datos (Data Seeding) ---
// Este bloque se ejecuta una vez al iniciar la aplicación para sembrar datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();


        // Aplica las migraciones pendientes a la base de datos.
        // Es una buena práctica para asegurar que la DB esté actualizada al iniciar la app.
        context.Database.Migrate();

        // Llama a tu método DataSeeder para insertar los productos iniciales
        await DataSeeder.SeedProductsAsync(context);
    }
    catch (Exception ex)
    {
        // Si ocurre un error durante la siembra de datos, lo logueamos
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar la base de datos.");
    }
}
// --- Fin del bloque de siembra de datos ---

app.Run();