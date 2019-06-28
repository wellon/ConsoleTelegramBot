using ConsoleTelegramBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;

namespace ConsoleTelegramBot.Services
{
    class MessagesService : IMessagesService
    {
        private readonly IBotService botService;

        public MessagesService(IBotService botService)
        {
            this.botService = botService;
        }

        public void SetupBot()
        {
            botService.Client.OnMessage += Bot_OnMessage;
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Text == null) return;

            foreach (var c in botService.Commands)
            {
                if (c.Contains(message))
                {
                    await c.Execute(message, botService.Client);
                }
            }
        }
    }
}
