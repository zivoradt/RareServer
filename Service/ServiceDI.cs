using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RareServer.Config;
using RareServer.Service.ChartServices;
using Sprache;

namespace RareServer.Service
{
    public static class ServiceDI
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<ApiSettings>(options =>
            {
                options.ApiBase = Environment.GetEnvironmentVariable("API_BASE_URL");
                options.ApiGet = $"gettimeentries?code={Environment.GetEnvironmentVariable("API_GET_CODE")}";
            });
            services.AddScoped<IPercentageTextDrawer, PercentageTextDrawer>();

            services.AddScoped<IChartSegmentDrawer, ChartSegmentDrawer>();

            services.AddScoped<INameDrawer, NameDrawer>();

            services.AddScoped<IChartService, ChartService>();

            services.AddScoped<ITimeEntryService, TimeEntryService>();

            services.AddHttpClient<ITimeEntryService, TimeEntryService>((serviceProvider, client) =>
            {
                var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

                client.BaseAddress = new Uri(apiSettings.ApiBase);
            });

            return services;
        }
    }
}