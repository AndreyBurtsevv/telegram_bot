using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands.Admin
{
    public class DownloadNewMemeCommand : Command
    {
        public DownloadNewMemeCommand(string command = "Download new Meme", string description = "Download new picture meme") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            if (!AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                return;
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                                        text: "Enter the name of the meme. *\"New meme {Yout word}\"*\nFind a meme with the word frog send *\"New meme frog\"*",
                                                         parseMode: ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
