using HomeValueHub.AI.DependencyInjection;
using HomeValueHub.API.Services;

namespace HomeValueHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add configuration root
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            // Add services to the container
            builder.Services.AddHvhEstimator(config);
            builder.Services.AddScoped<EstimateService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://localhost:7210")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });

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
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors();
            app.Run();
        }
    }
}