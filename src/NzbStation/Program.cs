using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using NzbStation.Data;
using NzbStation.Extensions;
using NzbStation.Tmdb;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace NzbStation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = CreateLogger();

            try
            {
                using var host = BuildHost(args);

                await host.MigrateDatabaseAsync();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application startup failed.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static Logger CreateLogger() =>
            new LoggerConfiguration()
                .WriteTo.Console(
                    outputTemplate: Constants.ConsoleLogFormat,
                    theme: AnsiConsoleTheme.Code)
                .Enrich.FromLogContext()
                .CreateLogger();

        private static IHost BuildHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(x =>
                    x.UseStartup<Program>())
                .UseSerilog()
                .Build();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProblemDetails();

            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<Database>();

            services.AddZynapse<Program>();

            services.AddMvcCore().AddJsonOptions(json =>
            {
                json.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                json.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddHttpClient<TmdbClient>(x =>
            {
                x.BaseAddress = new Uri("https://api.themoviedb.org/");
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhZWUyMTU1ZTBmOWEyYzIxMDdiMTkyNGE1NTMxMzhkMiIsInN1YiI6IjRmZGYzNjg4NzYwZWUzMWM0ODAwNDBiZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.m5UO7Z7H89fmTIHw1ocrQEpqVMgV2jL6Y_CwUG873bg");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
