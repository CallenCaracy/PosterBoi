using Microsoft.EntityFrameworkCore;
using PosterBoi.API.Extensions;
using PosterBoi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingConfiguration();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PosterBoiDBConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowedFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();