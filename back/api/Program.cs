using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using stock.dal.database;
using stock.domain.entities;
using stock.domain.services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Env.Load();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Modify config variable that will be injected by Services.AddSingleton<IConfiguration>(configuration)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>{
        string? jwt_key = configuration["JWT_KEY"] ?? throw new Exception("Missing JWT_KEY configuration");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT_ISSUER"],
            ValidAudience = configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt_key))
        };


        // extract token from cookies and place it into the Bearer.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? jwt_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME configuration");
                if (context.Request.Cookies.ContainsKey(jwt_name))
                {
                    context.Token = context.Request.Cookies[jwt_name];
                }
                return Task.CompletedTask;
            }
        };
    });

// Polices
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("AdminOnly", policy => policy.RequireClaim(nameof(User.Role),"Admin"));
});

// DB
builder.Services.AddScoped<IDataContext,DataContext>();

//Services 
builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddTransient<IHashService,HashService>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IProductService,ProductService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

RouteConfig.RegisterRoutes(app);

app.Run();