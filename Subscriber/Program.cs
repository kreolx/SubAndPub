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


namespace Subscriber
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
                .Services.AddSingleton<IMessageDbContext, MessageDbContext>()
                .AddTransient<IMessageService, MessageService>()
                .AddSingleton<ITransportService, TransportService>()
                .AddScoped<ISubscriberManager, SubscriberManager>()
                .AddAutoMapper(typeof(AutoMapping))
                .AddOptions<NatsConnectionSettings>()
                .Bind(configuration.GetSection(nameof(NatsConnectionSettings)));

            var serviceProvider = serviceCollection
                .Services.BuildServiceProvider();
            var subscriberManager = serviceProvider.GetService<ISubscriberManager>();
            await subscriberManager.ExecuteAsync();
            
            // using (var client = new NatsClient(connectoionInfo))
            // {
            //     client.Events.OfType<ClientConnected>()
            //         .Subscribe(ev => Console.WriteLine("Client connected"));
            //     await client.ConnectAsync();
            //     while (true)
            //     {
            //         await client.SubAsync("Random", stream => stream.Subscribe(msg =>
            //         {
            //             Console.WriteLine(msg.GetPayloadAsString());
            //         }));   
            //     }
            // }
            Console.ReadKey();
        }
    }
}
