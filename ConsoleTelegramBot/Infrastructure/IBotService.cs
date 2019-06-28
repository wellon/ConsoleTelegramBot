using ConsoleTelegramBot.Models.Commands;
using System.Collections.Generic;
using Telegram.Bot;

namespace ConsoleTelegramBot.Infrastructure
{
    interface IBotService
    {
        TelegramBotClient Client { get; }
        IReadOnlyList<Command> Commands { get; }
    }
}
