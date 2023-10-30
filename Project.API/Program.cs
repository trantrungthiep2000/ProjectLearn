using Project.API.Extenstions;

var builder = WebApplication.CreateBuilder(args);

builder.WebApplicationBuilderInstaller(typeof(Program));

var app = builder.Build();

app.WebApplicationInstaller(typeof(Program));

app.Run();