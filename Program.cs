using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;

var builder = WebApplication.CreateBuilder(args);

// 1. DODAJ SERVISE
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// POPRAVLJENO: Ovde je bila greška (tačka viška)
builder.Services.AddScoped<IFirmaService, FirmaService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 2. KONFIGURIŠI PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
