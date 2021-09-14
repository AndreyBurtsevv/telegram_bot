using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands
{
    class StartCommand : Command
    {
        public StartCommand(string command = "/start", string description = "Start for client") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                int num = new Random().Next(0, 6);
                await botClient.SendPhotoAsync(chatId: e.Message.Chat, System.IO.File.OpenRead(@$"Files/Starts/Start{num}.jpg"), replyMarkup: BotService.StartMenu());

                if (AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Hi Admin !", replyMarkup: BotService.StartMenuForAdmin());
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
