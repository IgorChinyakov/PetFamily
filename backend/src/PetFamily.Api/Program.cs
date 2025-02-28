using PetFamily.Application;
using PetFamily.Infrastructure;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure().AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Theme.NordDark);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
