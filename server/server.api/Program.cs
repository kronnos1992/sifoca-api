using Microsoft.EntityFrameworkCore;
using server.api.Services.Context;
using server.api.Services.Contracts;
using server.api.Services.Functions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(
        x =>
            x.SerializerSettings.ReferenceLoopHandling
            =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
    ); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SifocaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("default"))
    .EnableSensitiveDataLogging()
    .EnableThreadSafetyChecks()
);

builder.Services.AddScoped<IMovimentoContract, MovimentoFunctions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
