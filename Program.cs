using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromptVault.Api.Database;
using PromptVault.Api.Database.DatabaseSeeder;
using PromptVault.Api.Middleware;
using PromptVault.Api.Configuration;
using PromptVault.Api.Services;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Register all FluentValidation validators from this assembly
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // Add automapper and config with the AutoMapperProfile.cs class
            builder.Services.AddAutoMapper(cfg =>
                cfg.AddProfile<PromptVault.Api.AutoMapper.AutoMapperProfile>());

            // Bind OpenAI settings from config/user secrets and register HttpClient for OpenAiService
            builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection("OpenAI"));
            builder.Services.AddHttpClient<IOpenAiService, OpenAiService>();

            // Register services with interface bindings
            builder.Services.AddScoped<IPromptService, PromptService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITestResultService, TestResultService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //Connection string
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

            // Database connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Start seeder when program starts
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                Seeder.Seed(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Global exception handler — must be first to catch all exceptions
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
