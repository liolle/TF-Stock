using DotNetEnv;
using stock.dal.database;
using stock.domain.services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Env.Load();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Modify config variable that will be injected by Services.AddSingleton<IConfiguration>(configuration)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// DB
builder.Services.AddScoped<IDataContext,DataContext>();
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

app.UseHttpsRedirection();

RouteConfig.RegisterRoutes(app);

app.Run();