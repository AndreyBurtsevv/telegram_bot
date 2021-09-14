using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands.Admin
{
    class ChangeMemeNameCommand : Command
    {
        public ChangeMemeNameCommand(string command = "Change Meme name", string description = "Change name of the Meme") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            if (!AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                return;
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                                        text: "Enter the new name for the meme.\n*\"Replacement {type} Old:{old name} New:{new name}\"*\nType: Audio, Picture.",
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
