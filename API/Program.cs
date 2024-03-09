using Microsoft.EntityFrameworkCore;
using Serilog;
using API.Data;
using API.Interfaces;
using API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/productLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// builder.Host.UseSerilog();

builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddCors();

builder.Services.AddScoped<ITokenService, TokenService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(cors =>
{
    cors.AllowAnyHeader();
    cors.AllowAnyMethod();
    // cors.WithOrigins("https://localhost:4200", "https://localhost:5001", "http://localhost:5000");
    cors.WithOrigins("https://localhost:4000", "http://localhost:8080", "http://localhost:4200", "http://localhost:80");
});

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}

app.Run();
