using ConsoleTelegramBot.Configuration;
using ConsoleTelegramBot.Infrastructure;
using ConsoleTelegramBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ConsoleTelegramBot
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            BuildConfiguration();
            RegisterServices();

            Start();

            Console.ReadLine();
            DisposeServices();
        }

        private static void Start()
        {
            serviceProvider.GetService<IMessagesService>().SetupBot();
            var botService = serviceProvider.GetService<IBotService>();

            botService.Client.StartReceiving();

            var me = botService.Client.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}");
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<BotConfiguration>(configuration.GetSection("BotConfiguration"));
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<IMessagesService, MessagesService>();
            serviceProvider = services.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose();
            }
        }
    }
}
