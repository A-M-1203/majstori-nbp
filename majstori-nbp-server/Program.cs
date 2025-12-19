using System.Text;
using dotenv.net;
using majstori_nbp_server.Helper;
using majstori_nbp_server.Implementations;
using majstori_nbp_server.Middleware;
using majstori_nbp_server.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Neo4j.Driver;
using StackExchange.Redis;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

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

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            // use AllowCredentials() only if you really need cookies/auth
            //.AllowCredentials()
            .WithOrigins(
                "http://localhost:4050",
                "http://127.0.0.1:4050"
            );
    });
});


builder.Services.AddSingleton<IDriver>(dr =>
{
    var uri = Environment.GetEnvironmentVariable("NEO4J_URI");
    var user = Environment.GetEnvironmentVariable("NEO4J_USERNAME");
    var password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD");
    return GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
});

builder.Services.AddSingleton<IDatabase>(dr =>
{
    var uri = Environment.GetEnvironmentVariable("REDIS_URI");
    var port= int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT"));
    var username= Environment.GetEnvironmentVariable("REDIS_USERNAME");
    var password=Environment.GetEnvironmentVariable("REDIS_PASSWORD");
    var connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints =
        {
            { uri, port }
        },
        User = username,
        Password = password
    });
    IDatabase _redisDb = connection.GetDatabase();
    return _redisDb;
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IMajstorService, MajstorService>();
builder.Services.AddScoped<IKlijentService, KlijentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IKategorijaService, KategorijaService>();
builder.Services.AddScoped<IOcenaService, OcenaService>();
builder.Services.AddScoped<JwtSecurityTokenHandlerWrapper>();
builder.Services.AddScoped< JwtAuthorizeFilter>();

builder.Services.AddControllersWithViews();



var app = builder.Build();
app.UseCors("AllowAngular");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();



app.UseAuthorization();

app.UseAuthentication();

app.UseStaticFiles();

app.Run();