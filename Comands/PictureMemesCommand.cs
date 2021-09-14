using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands
{
    class PictureMemesCommand : Command
    {
        public PictureMemesCommand(string command = "Picture Memes", string description = "Get all pcture memes") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Picture Memes:", replyMarkup: BotService.ImagesButtons());
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
