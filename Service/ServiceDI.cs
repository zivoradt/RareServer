using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RareServer.Config;

namespace RareServer.Service
{
    public static class ServiceDI
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<ApiSettings>(configurationManager.GetSection("API"));

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