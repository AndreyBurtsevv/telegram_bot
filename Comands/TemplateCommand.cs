using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramMemBot.Comands
{
    class TemplateCommand : Command
    {
        public TemplateCommand(string command = "Template", string description = "Find mem by template") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                var template = new string(e.Message.Text.Skip(9).ToArray());
                var list = new List<List<InlineKeyboardButton>>();
                string name;

                foreach (var path in Directory.GetFiles(@$"Files/Photos"))
                {
                    name = Path.GetFileName(path);
                    name = name.Substring(0, name.LastIndexOf('.'));

                    if (name.Contains(template, StringComparison.CurrentCultureIgnoreCase))
                        list.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: "Picture Memes: " + name, callbackData: "I" + path) });
                }

                foreach (var path in Directory.GetFiles(@$"Files/Audios"))
                {
                    name = Path.GetFileName(path);
                    name = name.Substring(0, name.LastIndexOf('.'));

                    if (name.Contains(template, StringComparison.CurrentCultureIgnoreCase))
                        list.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: "Audio Mems: " + name, callbackData: "A" + path) });
                }

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"Your template - {template}", replyMarkup: new InlineKeyboardMarkup(list));
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
