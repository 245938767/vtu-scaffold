using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VTU.Data.Models;
using VTU.Infrastructure;
using VTU.Infrastructure.Extension;
using VTU.Infrastructure.Filters;
using VTU.Service.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppService();
builder.Services.AddControllers(x => x.Filters.Add<GlobalExceptionFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new AppSettingInstants(builder.Configuration));
// init DB ORM
builder.Services.AddDbContext<EntityDbContext>((_, o) => DataServiceCollectionExtensions.GetSql(o));
builder.Services.AddSingleton<Func<EntityDbContext>>(s =>
{
    var options = DataServiceCollectionExtensions.GetSql(new DbContextOptionsBuilder<EntityDbContext>()).Options;
    return () => new EntityDbContext(options, s.GetServices<DbModule>());
});
// add Module
builder.Services.AddDbModules();

//jwt 
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddCookie()
    .AddJwtBearer(o => { o.TokenValidationParameters = JwtHelper.ValidParameters(); });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth", Version = "v1" });

    #region Swagger Authentication

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description =
            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    #endregion
});
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