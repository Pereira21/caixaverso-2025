using InvestimentosCaixa.Api.Config;
using InvestimentosCaixa.Api.Models.Simulacao;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // Adiciona o esquema de seguranca para JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira somente o token"
    });

    // Adiciona a exigencia de seguranca global
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
            new string[] {}
        }
    });
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

var app = builder.Build();

// Aplica todas as migrations no banco
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<InvestimentosCaixaDbContext>();
    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
    await SeedIdentityAsync(services);
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TelemetriaMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedIdentityAsync(IServiceProvider services)
{
    var userManager = services.GetRequiredService<UserManager<IdentityUser<Guid>>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    var analistaRoleId = Guid.Parse("D0149110-916F-475E-ACFE-6DE68929DB7F");
    var adminRoleId = Guid.Parse("A072D7DC-F5BA-43B0-8B1E-12591FD3585C");

    // 1) Cria role admin
    var roleAdminName = "admin";
    if (!await roleManager.RoleExistsAsync(roleAdminName))
    {
        var role = new IdentityRole<Guid>(roleAdminName);
        role.Id = adminRoleId;
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

    // 3) Cria usuário admin
    var adminEmail = "usuario@admin.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new IdentityUser<Guid>
        {
            Id = Guid.Parse("46ECE551-FEB3-45D7-A800-4980EC840D9B"),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = adminEmail,
            NormalizedEmail = adminEmail.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var createAdmin = await userManager.CreateAsync(admin, "@Admin123");
        if (!createAdmin.Succeeded)
            throw new Exception("Erro ao criar admin: " + string.Join("; ", createAdmin.Errors.Select(e => e.Description)));
    }

    // Vincula analista à role analista
    if (!await userManager.IsInRoleAsync(analista, roleAnalistaName))
    {
        await userManager.AddToRoleAsync(analista, roleAnalistaName);
    }

    // Vincula admin à role admin
    if (!await userManager.IsInRoleAsync(admin, roleAdminName))
    {
        await userManager.AddToRoleAsync(admin, roleAdminName);
    }
}