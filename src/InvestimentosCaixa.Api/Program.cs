using InvestimentosCaixa.Api.Config;
using InvestimentosCaixa.Api.Models.Simulacao;
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

var builder = WebApplication.CreateBuilder(args);

var dataBase = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options =>
    options.UseSqlServer(dataBase));

// Configurando Identity
builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<InvestimentosCaixaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ResolveDependencies(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    //Model -> DTO
    cfg.CreateMap<SimularInvestimentoModel, SimularInvestimentoRequest>();

    //Entidade -> DTO
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
});

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
    db.Database.Migrate();
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

    var usuarioId = Guid.Parse("3f9c4c9e-5c42-48d0-bb42-2adcb324fa73");
    var adminId = Guid.Parse("f27d5c20-6a7e-4b75-9e2f-5c4e99f93712");

    // 1) Cria role admin
    var roleName = "admin";
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        var role = new IdentityRole<Guid>(roleName);
        role.Id = adminId;
        await roleManager.CreateAsync(role);
    }

    // 2) Cria usuário padrão (usuario)
    var usuarioEmail = "usuario@teste.com";
    var usuario = await userManager.FindByEmailAsync(usuarioEmail);
    if (usuario == null)
    {
        usuario = new IdentityUser<Guid>
        {
            Id = usuarioId,
            UserName = "usuario",
            NormalizedUserName = "USUARIO",
            Email = usuarioEmail,
            NormalizedEmail = usuarioEmail.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(usuario, "@User123");
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new Exception($"Erro ao criar usuário 'usuario': {errors}");
        }
    }

    // 3) (opcional) Cria um usuário admin e vincula role admin
    var adminEmail = "admin@admin.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new IdentityUser<Guid>
        {
            Id = adminId,
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

    // Vincula admin à role admin
    if (!await userManager.IsInRoleAsync(admin, roleName))
    {
        await userManager.AddToRoleAsync(admin, roleName);
    }
}