using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // REQUIRED for API

var app = builder.Build();

app.UseStaticFiles(); // serve index.html
app.MapControllers();

app.Run();