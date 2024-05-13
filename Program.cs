using APISecurityWithAPIKeys.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( x =>
{
    x.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Description = "Api Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = AuthConfig.ApiKeyHeader,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement()
    {
        {scheme, new List<string>() }
    };

    x.AddSecurityRequirement(requirement);
});

builder.Services.AddScoped<ApiKeyAuthenticationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//T�m controller'lara eri�im api-key ile yap�lmak isteniyorsa bu yorum sat�r� a��l�p builder.Services.AddScoped<ApiKeyAuthenticationFilter>(); bu k�s�m yorum sat�r� yap�lmal�
//app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
