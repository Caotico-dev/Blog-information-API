using Blog_informetion_API.Models;
using Blog_informetion_API.DirectoryController;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;


Log.Logger = new LoggerConfiguration()
     .MinimumLevel.Error()
     .WriteTo.MongoDBBson(cfg =>
     {
         cfg.SetConnectionString("mongodb://localhost:27017/Logs");
         cfg.SetCreateCappedCollection(100);
         cfg.SetRollingInternal(Serilog.Sinks.MongoDB.RollingInterval.Day);
     })
    .CreateLogger();
Log.Information("Starting web application");

try
{


    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services.AddTransient<IDirectoryController, DirectoryInformation>();

    builder.Services.AddDbContext<BlogInformationApiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;

    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
    builder.Services.AddAuthorization(options => options.AddPolicy("adminPolicy", policy => policy.RequireRole("Admin")));
    builder.Services.AddAuthentication();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IBlog_information_SQL, Blog_Information_API_SQL>();
    builder.Services.AddScoped<IDirectoryController, DirectoryInformation>();



    var app = builder.Build();

    //pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();


    }
    app.UseCors();
    app.UseStaticFiles();
    app.MapFallbackToFile("login.html");
    app.MapControllers();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");

}
finally
{
    Log.CloseAndFlush();
}
