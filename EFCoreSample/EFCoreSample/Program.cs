using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.RateLimiting;
using EFCoreSample.ActionFilter;
using EFCoreSample.DbContext;
using EFCoreSample.Interceptor;
using EFCoreSample.Jwt;
using EFCoreSample.Middleware;
using EFCoreSample.Models;
using EFCoreSample.ServiceCollection;
using EFCoreSample.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AspNetCoreFeature;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(option =>
        {
            option.Filters.Add<ValidationModelActionFilter>();
            option.Filters.Add<ApiResponseActionFilter>();
        });
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
        // http logging
        builder.Services.AddCustomHttpLogging();
        builder.Services.AddHttpLoggingInterceptor<HttpLoggingInterceptor>();
        
        // Db Context
        builder.Services.AddDbContext<EFcoreSampleContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreSampleConnectionString"));
        });
        
        
        var app = builder.Build();
        app.UseMiddleware<ExceptionMiddleware>();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = (httpContext, healthReport) =>
            {
                var result = new
                {
                    health = Enum.Parse<HealthStatus>(healthReport.Status.ToString()).ToString(),
                    services = healthReport.Entries.Select(item => new
                    {
                        name = item.Key,
                        health = Enum.Parse<HealthStatus>(item.Value.Status.ToString()).ToString()
                    })
                };
                httpContext.Response.ContentType = "application/json";
                var json = System.Text.Json.JsonSerializer.Serialize(result);
                return httpContext.Response.WriteAsync(json);
            }
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseHttpLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();

        app.MapControllers();
        app.Run();
    }
}