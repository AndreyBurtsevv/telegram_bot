using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands
{
    class AudioMemesCommand : Command
    {
        public AudioMemesCommand(string command = "Audio Memes", string description = "Get all audio memes") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Audio Memes:", replyMarkup: BotService.AudiosButtons());
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
