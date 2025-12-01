using System.Text;
using majstori_nbp_server.Implementations;
using majstori_nbp_server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<DateTime>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("15.10.2025 21:37:45")
    });
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IMajstorService, MajstorService>();
builder.Services.AddScoped<IKlijentService, KlijentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:TokenKey"]!)
            ),
            ValidateIssuerSigningKey = true
        };
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