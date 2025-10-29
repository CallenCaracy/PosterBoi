using PosterBoi.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var startup = new PosterBoi.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.AddLoggingConfiguration();

var app = builder.Build();

startup.Configure(app);

app.Run();