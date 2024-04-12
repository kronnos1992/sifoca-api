using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;
using server.api.Services.Functions;
using FastReport.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(op =>
{
    //politica de autenticação
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    op.Filters.Add(new AuthorizeFilter(policy));
})
    //politica de controlo de loops e serialização
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling
        = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
        .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
        = new DefaultContractResolver())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.DefaultBufferSize = 1024;
        options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.ReadCommentHandling = new JsonCommentHandling();
        options.JsonSerializerOptions.UnknownTypeHandling = new JsonUnknownTypeHandling();
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "sifoca-api",
        Version = "v1.0.1",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Nossos Contactos",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Licença",
            Url = new Uri("https://example.com/license")
        }
    });
    //                                                                                                                                                      var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //s.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization using the Bearer Scheme. 
            Enter 'Bearer' [space].Example:\ 'Bearer 12345abcdef\'",
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors();

builder.Services.AddDataProtection()
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(p =>
    {
#pragma warning disable CS8604 // Possible null reference argument.
        p.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
                //(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                (Encoding.UTF8
                .GetBytes(builder.Configuration["Jwt:Key"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // jwt definitions
            ValidateIssuer = false, 
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateTokenReplay = true,
            ClockSkew = TimeSpan.Zero,
            SaveSigninToken = true
        };
#pragma warning restore CS8604 // Possible null reference argument.
        p.AutomaticRefreshInterval = TimeSpan.FromSeconds(10);
    });

builder.Services.AddAuthentication(p =>
    p.RequireAuthenticatedSignIn = true
);

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

IdentityBuilder _builder = builder.Services.AddIdentityCore<AppUser>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequiredLength = 4;
    //o.Password.RequiredUniqueChars = 3;
}).AddRoles<AppRole>();
//politica de roles e usuarios
_builder = new IdentityBuilder(_builder.UserType, typeof(AppRole), builder.Services);
_builder.AddEntityFrameworkStores<SifocaContext>();
_builder.AddRoleValidator<RoleValidator<AppRole>>();
_builder.AddRoleManager<RoleManager<AppRole>>();
_builder.AddSignInManager<SignInManager<AppUser>>();
//builder.Services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
_builder.Services.AddAuthorization();

// builder.Services.AddDbContext<SifocaContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("default"))
// );
builder.Services.AddFastReport();

builder.Services.AddDbContext<SifocaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("memory"))
    //.AddInterceptors()
);

// habilitar o consumo da BD ao FastReport
FastReport.Utils.RegisteredObjects.AddConnection(typeof(SQLiteDataConnection));

builder.Services.AddScoped<ISaidasContract, SaidasFunctions>();
builder.Services.AddScoped<IEntradasContract, EntradasFunctions>();
builder.Services.AddScoped<IAcessoContract, AcessoFunctions>();
builder.Services.AddScoped<IResumoContract, ResumoFunctions>();

var app = builder.Build();

app.UseCors(p =>
{
    p.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// habilitar a utilização do FastReport
app.UseFastReport();

//habilitar ficheiros estaticos
app.UseStaticFiles();

app.MapControllers();

app.Run();
