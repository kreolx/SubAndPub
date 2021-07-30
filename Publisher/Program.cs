using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SubAndPub.Commons.DAL.DbContexts;
using SubAndPub.Commons.DAL.Services;
using SubAndPub.Commons.Transport;
using SubAndPub.Commons.Transport.Services;

namespace Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var serviceCollection = new ServiceCollection()
                .AddLogging(b =>
                {
                    b.AddConsole();
                    b.SetMinimumLevel(LogLevel.Debug);
                })
                .AddOptions<DbConnectionSettings>().Bind(configuration.GetSection(nameof(DbConnectionSettings)))
                .Services.AddSingleton<MessageDbContext>()
                .AddTransient<IMessageService, MessageService>()
                .AddSingleton<ITransportService, TransportService>()
                .AddScoped<IPublisherManager, PublisherManager>()
                .AddAutoMapper(typeof(AutoMapping))
                .AddOptions<NatsConnectionSettings>()
                .Bind(configuration.GetSection(nameof(NatsConnectionSettings)));

            var serviceProvider = serviceCollection
                .Services.BuildServiceProvider();
            var publisherManager = serviceProvider.GetService<IPublisherManager>();
            
            await publisherManager.ExecuteAsync();
            
            Console.ReadKey();
        }
    }
}
