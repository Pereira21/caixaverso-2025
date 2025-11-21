using InvestimentosCaixa.Api.Config;
using InvestimentosCaixa.Api.Models.Produto;
using InvestimentosCaixa.Api.Models.Simulacao;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var dataBase = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options =>
    options.UseSqlServer(dataBase));

// Configurando Identity
builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<InvestimentosCaixaDbContext>()
    .AddDefaultTokenProviders();

//Dependencias
builder.Services.ResolveDependencies(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    #region Model -> DTO
    cfg.CreateMap<SimularInvestimentoModel, SimularInvestimentoRequest>();
    #endregion    

    #region Entidade <-> DTO
    cfg.CreateMap<Simulacao, SimulacaoResponseDTO>()
    .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty));

    cfg.CreateMap<Produto, ProdutoRecomendadoResponse>()
        .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
        .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TipoProduto != null ? src.TipoProduto.Nome : string.Empty))
        .ForMember(dest => dest.Rentabilidade, opt => opt.MapFrom(src => src.RentabilidadeAnual))
        .ForMember(dest => dest.Risco, opt => opt.MapFrom(src => src.TipoProduto != null && src.TipoProduto.Risco != null ? src.TipoProduto.Risco.Nome : string.Empty));

    cfg.CreateMap<Investimento, InvestimentoResponse>()
        .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Produto != null && src.Produto.TipoProduto != null ? src.Produto.TipoProduto.Nome : string.Empty))
        .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor))
        .ForMember(dest => dest.Rentabilidade, opt => opt.MapFrom(src => src.Rentabilidade))
        .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data.ToString("yyyy-MM-dd")));

    cfg.CreateMap<Produto, ProdutoDto>().ReverseMap();
    cfg.CreateMap<TipoProduto, TipoProdutoDto>().ReverseMap();
    cfg.CreateMap<Risco, RiscoDto>().ReverseMap();
    cfg.CreateMap<PerfilClassificacao, PerfilClassificacaoDto>().ReverseMap();
    cfg.CreateMap<PerfilRisco, PerfilRiscoDto>().ReverseMap();
    cfg.CreateMap<PerfilPontuacaoVolume, PerfilPontuacaoVolumeDto>().ReverseMap();
    cfg.CreateMap<PerfilPontuacaoFrequencia, PerfilPontuacaoFrequenciaDto>().ReverseMap();
    cfg.CreateMap<PerfilPontuacaoRisco, PerfilPontuacaoRiscoDto>().ReverseMap();

    #endregion

    #region Entidade -> Model
    cfg.CreateMap<TipoProduto, TipoProdutoDisponivelModel>()
    .ForMember(dest => dest.RiscoNome, opt => opt.MapFrom(src => src.Risco != null ? src.Risco.Nome : string.Empty));
    cfg.CreateMap<Produto, ProdutoDisponivelModel>();
    #endregion
});

#region Redis
var redisConn = builder.Configuration["Redis:Connection"];
var redisInstance = builder.Configuration["Redis:InstanceName"];

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConn;
    options.InstanceName = redisInstance;
});
#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InvestimentosCaixa API",
        Version = "v1",
        Description = "API para simulação e gestão de investimentos da Caixa Econômica Federal",
        Contact = new OpenApiContact
        {
            Name = "Equipe CaixaInvestimentos",
            Email = "dev@investimentos-caixa.com",
            Url = new Uri("https://github.com/Pereira21/caixaverso-2025")
        }
    });

    // Adiciona o esquema de segurança para JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira somente o token"
    });

    // Adiciona a exigência de segurança global
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});


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
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Muitas requisições. Tente novamente mais tarde!");
    };
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<InvestimentosCaixaDbContext>();

    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();

    await SeedIdentityAsync(services);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investimentos Caixa API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TelemetriaMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();

static async Task SeedIdentityAsync(IServiceProvider services)
{
    var userManager = services.GetRequiredService<UserManager<IdentityUser<Guid>>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    var analistaRoleId = Guid.Parse("D0149110-916F-475E-ACFE-6DE68929DB7F");
    var tecnicoRoleId = Guid.Parse("A072D7DC-F5BA-43B0-8B1E-12591FD3585C");

    // 1) Cria role tecnico
    var roleTecnicoName = "tecnico";
    if (!await roleManager.RoleExistsAsync(roleTecnicoName))
    {
        var role = new IdentityRole<Guid>(roleTecnicoName);
        role.Id = tecnicoRoleId;
        await roleManager.CreateAsync(role);
    }

    // 2) Cria role analista
    var roleAnalistaName = "analista";
    if (!await roleManager.RoleExistsAsync(roleAnalistaName))
    {
        var role = new IdentityRole<Guid>(roleAnalistaName);
        role.Id = analistaRoleId;
        await roleManager.CreateAsync(role);
    }

    // 2) Cria usuário analista
    var analistaEmail = "usuario@analista.com";
    var analista = await userManager.FindByEmailAsync(analistaEmail);
    if (analista == null)
    {
        analista = new IdentityUser<Guid>
        {
            Id = Guid.Parse("3966F0D0-B722-4D13-BB21-0E91CF8B6901"),
            UserName = "analista",
            NormalizedUserName = "ANALISTA",
            Email = analistaEmail,
            NormalizedEmail = analistaEmail.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(analista, "@Analista123");
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new Exception($"Erro ao criar usuário 'analista': {errors}");
        }
    }

    // 3) Cria usuário tecnico
    var tecnicoEmail = "usuario@tecnico.com";
    var tecnico = await userManager.FindByEmailAsync(tecnicoEmail);
    if (tecnico == null)
    {
        tecnico = new IdentityUser<Guid>
        {
            Id = Guid.Parse("46ECE551-FEB3-45D7-A800-4980EC840D9B"),
            UserName = "tecnico",
            NormalizedUserName = "TECNICO",
            Email = tecnicoEmail,
            NormalizedEmail = tecnicoEmail.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var createTecnico = await userManager.CreateAsync(tecnico, "@Tecnico123");
        if (!createTecnico.Succeeded)
            throw new Exception("Erro ao criar tecnico: " + string.Join("; ", createTecnico.Errors.Select(e => e.Description)));
    }

    // 3) Cria usuário comum
    var usuarioEmail = "usuario@usuario.com";
    var usuario = await userManager.FindByEmailAsync(usuarioEmail);
    if (usuario == null)
    {
        usuario = new IdentityUser<Guid>
        {
            Id = Guid.Parse("E6B7794B-9367-4B2A-AEF4-85B15FA42571"),
            UserName = "usuario",
            NormalizedUserName = "usuario",
            Email = usuarioEmail,
            NormalizedEmail = usuarioEmail.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var createUsuario = await userManager.CreateAsync(usuario, "@Usuario123");
        if (!createUsuario.Succeeded)
            throw new Exception("Erro ao criar usuario comum: " + string.Join("; ", createUsuario.Errors.Select(e => e.Description)));
    }

    // Vincula analista à role analista
    if (!await userManager.IsInRoleAsync(analista, roleAnalistaName))
    {
        await userManager.AddToRoleAsync(analista, roleAnalistaName);
    }

    // Vincula admin à role tecnico
    if (!await userManager.IsInRoleAsync(tecnico, roleTecnicoName))
    {
        await userManager.AddToRoleAsync(tecnico, roleTecnicoName);
    }
}

public partial class Program { }