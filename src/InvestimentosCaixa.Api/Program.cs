using InvestimentosCaixa.Api.Config;
using InvestimentosCaixa.Api.Models.Simulacao;
using InvestimentosCaixa.Application.DTO.Request;
using InvestimentosCaixa.Application.DTO.Response;
using InvestimentosCaixa.Domain.Entidades;
using InvestimentosCaixa.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var dataBase = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options =>
    options.UseSqlServer(dataBase));

builder.Services.ResolveDependencies(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    //Model -> DTO
    cfg.CreateMap<SimularInvestimentoModel, SimularInvestimentoRequest>();

    //Entidade -> DTO
    cfg.CreateMap<Simulacao, SimulacaoResponseDTO>()
        .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TelemetriaMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
