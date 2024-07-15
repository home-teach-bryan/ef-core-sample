using EFCoreSample.ActionFilter;
using EFCoreSample.DbContext;
using EFCoreSample.Jobs;
using EFCoreSample.Jwt;
using EFCoreSample.Middleware;
using EFCoreSample.Models;
using EFCoreSample.ServiceCollection;
using EFCoreSample.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EFCoreSample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        try
        {
            Log.Information("Starting Web Application");
            
            // Add services to the container.
            builder.Services.AddControllers(option => { option.Filters.Add<ValidationModelActionFilter>(); });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.Configure<ApiBehaviorOptions>(item => item.SuppressModelStateInvalidFilter = true);
            // swagger document spec 
            builder.Services.AddCustomSwaggerGen();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<JwtTokenGenerator>();

            // jwt authentication setting
            builder.Services.AddCustomJwtAuthentication(builder.Configuration);

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // rate limit
            builder.Services.AddCustomRateLimiter();

            // health check
            builder.Services.AddCustomHealthCheck();
            
            // logging
            builder.Services.AddSerilog();

            // Db Context
            builder.Services.AddDbContext<EFcoreSampleContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreSampleConnectionString"));
            });
            
            // hangfire
            builder.Services.AddHangfire(option => option.UseInMemoryStorage());
            builder.Services.AddHangfireServer();
            builder.Services.AddSingleton<HangFireJobManager>();

            var app = builder.Build();
            
            var jobManager = app.Services.GetRequiredService<HangFireJobManager>();
            jobManager.RegisterJobs();
            
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCustomHealthCheck();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHangfireDashboard("/hangfire");
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();

            app.MapControllers();
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
    }
}