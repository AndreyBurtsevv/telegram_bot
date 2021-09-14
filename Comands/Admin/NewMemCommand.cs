using Serilog;
using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands.Admin
{
    public class NewMemCommand : Command
    {
        public NewMemCommand(string command = "New meme", string description = "Download new picture meme") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                if (!AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                    return;

                var newName = new string(e.Message.Text.Skip(9).ToArray());
                AdminService.NameNevFile = newName;

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                   text: $"*{newName}* - Name of new Meme.\nSend me a photo or audio.",
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
