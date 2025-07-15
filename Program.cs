using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Implementations; // Para tus implementaciones de servicio
using SolYSalEcommerce.Services.Interfaces;     // ¡NUEVO! Necesario para tus interfaces de servicio (IAuthService, IProductService, etc.)
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => // <-- ¡Cambiado a IdentityRole<Guid>!
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<IdentityRole<Guid>>() // ¡¡¡NUEVO!!! Esta línea registra RoleManager con el tipo de rol correcto
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

// Register your application services here
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// No se registra ningún IPaymentService o IWompiService aquí

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ¡NUEVO! Habilita el servicio de archivos estáticos (imágenes, CSS, JS, etc.)
app.UseStaticFiles(); // Asegúrate de que esta línea esté aquí si quieres servir imágenes desde wwwroot

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
        // Opcional y solo para desarrollo: Elimina la base de datos y la recrea desde cero.
        // Solo descomenta si necesitas reiniciar la DB completamente en cada inicio.
        // context.Database.EnsureDeleted(); // Cuidado al usar esto en producción o con datos importantes

        // Aplica las migraciones pendientes a la base de datos.
        // Es redundante si ya ejecutas 'Update-Database' manualmente, pero asegura que esté actualizada.
        context.Database.Migrate();

        // Llama a tu método DataSeeder para insertar los productos
        await DataSeeder.SeedProductsAsync(context);
    }
    catch (Exception ex)
    {
        // Si ocurre un error durante la siembra, lo logueamos
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al sembrar la base de datos.");
    }
}
// --- Fin del bloque de siembra de datos ---

app.Run();