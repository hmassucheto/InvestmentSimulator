using InvestmentSimulator.Domain.Configuration;
using InvestmentSimulator.Domain.Interfaces;
using InvestmentSimulator.Domain.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<InvestmentSettings>(
    builder.Configuration.GetSection("InvestmentSettings"));
builder.Services.AddScoped<ITaxCalculator, CdbTaxCalculator>();
builder.Services.AddScoped<IInvestmentCalculator, InvestmentCalculator>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

await app.RunAsync();
