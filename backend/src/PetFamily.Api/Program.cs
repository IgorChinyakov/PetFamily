using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.Web;
using PetFamily.Web.Middlewares;
using Serilog;
using Serilog.Events;
using SwaggerThemes;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

//��� ������
//builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // ��������� ����� �����������
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "������� JWT �����, ��������� ������: Bearer {�����}"
    });

    // ��������� ��� ����� �� ���� ��������� �� ���������
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

builder.Services.AddSerilog();

builder.Services
    .AddFilesModule(builder.Configuration);

builder.Services
    .AddVolunteersModule(builder.Configuration);

builder.Services
    .AddSpeciesModule(builder.Configuration);

builder.Services
    .AddAccountsModule(builder.Configuration);

builder.Services
    .AddFramework();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Theme.NordDark);

    await app.ExecuteSeeder();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }
