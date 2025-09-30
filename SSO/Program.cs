using AutoMapper;
using GuardarArchivos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DAL.DBContext;
using SistemaERP.IOC;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Utilidades;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
//Se agrega el mapeo del SSO context a la BD
builder.Services.AddDbContext<SSOContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ssoConection"));
});
//Se agrega el mapeo del automapper
builder.Services.AddSingleton(provIder =>
    new MapperConfiguration(config =>
    {
        config.AddProfile(new AutoMapperProfile());
    }).CreateMapper());
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");
//Se agrega el CORS
builder.Services.AddCors(zOptions =>
{
    //zOptions.AddDefaultPolicy(zBuilder =>
    //{
    //    zBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    //      .WithExposedHeaders(new string[] { "CantidadTotalRegistros" });
    //});

    var allowedHosts = builder.Configuration.GetSection("AllowedHosts").Get<string[]>();

    if (allowedHosts != null && allowedHosts.Length > 0)
    {
        zOptions.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(allowedHosts)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    }
    else
    {
        // Configuración de CORS por defecto si no hay AllowedHosts configurados
        zOptions.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    }


});
builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        // (opcional) políticas de password/lockout
        // options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SSOContext>()
    .AddSignInManager();               // Para usar CheckPasswordSignInAsync

// Autenticación por defecto = JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(zOpciones =>
{
    zOpciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
        ClockSkew = TimeSpan.Zero
    };
});


//Se agrega el mapeo de los controladores con la clase para filtrar errores
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroExcepcion));
});
builder.Services.AddEndpointsApiExplorer();
//Se agrega la autorización del swagger con la api
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token} \")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
//Se agregan los servicios con sus interfaces
builder.Services.InyectarPolicyEmpresa(builder.Configuration);
builder.Services.InyectarDependencias(builder.Configuration);
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
