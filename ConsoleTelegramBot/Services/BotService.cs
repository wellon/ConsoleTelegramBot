using ConsoleTelegramBot.Configuration;
using ConsoleTelegramBot.Infrastructure;
using ConsoleTelegramBot.Models.Commands;
using Microsoft.Extensions.Options;
using MihaZupan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot;

namespace ConsoleTelegramBot.Services
{
    public class BotService : IBotService
    {
        private readonly BotConfiguration _config;
        public TelegramBotClient Client { get; }
        private static List<Command> commandsList;
        public IReadOnlyList<Command> Commands
        {
            get
            {
                return commandsList.AsReadOnly();
            }
        }

        public BotService(IOptions<BotConfiguration> config)
        {
            _config = config.Value;
            // use proxy if configured in appsettings.*.json
            Client = string.IsNullOrEmpty(_config.Socks5Host)
                ? new TelegramBotClient(_config.BotToken)
                : new TelegramBotClient(
                    _config.BotToken,
                    new HttpToSocks5Proxy(_config.Socks5Host, _config.Socks5Port, _config.Socks5UserName, _config.Socks5Password));

            InitCommands();
        }

        private void InitCommands()
        {
            Type commandType = typeof(Command);

            commandsList = new List<Command>();

            foreach (Type type in Assembly.GetAssembly(commandType).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(commandType)))
            {
                commandsList.Add((Command)Activator.CreateInstance(type));
            }
        }
    }
}
