using InvestimentosCaixa.Api.Config;
using InvestimentosCaixa.Api.Models;
using InvestimentosCaixa.Application.DTO;
using InvestimentosCaixa.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dataBase = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options =>
    options.UseSqlServer(dataBase));

builder.Services.ResolveDependencies(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<SimularInvestimentoModel, SimularInvestimentoRequestDTO>();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
