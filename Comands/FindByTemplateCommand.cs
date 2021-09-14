using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramMemBot.Comands
{
    public class FindByTemplateCommand : Command
    {
        public FindByTemplateCommand(string command = "Find By Template", string description = "Find mem by template") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                            text: "Enter the word associated with the mem. *\"Template {Your word}\"*\nFind a meme with the word frog send *\"Template frog\"*",
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
