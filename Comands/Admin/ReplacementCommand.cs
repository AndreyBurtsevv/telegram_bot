using Serilog;
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands.Admin
{
    public class ReplacementCommand : Command
    {
        public ReplacementCommand(string command = "Replacement", string description = "Replacement Meme name") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            if (!AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                return;

            try
            {
                string oldName = e.Message.Text[(e.Message.Text.IndexOf("Old:") + "Old:".Length)..].Trim(); ;
                oldName = oldName.Remove(oldName.IndexOf(" New:"));
                string newName = e.Message.Text[(e.Message.Text.IndexOf("New:") + "New:".Length)..].Trim(); ;

                if (e.Message.Text.Contains("Audio", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (var path in Directory.GetFiles(@$"Files/Audios"))
                    {
                        string fileName = Path.GetFileName(path);

                        if (oldName == fileName.Substring(0, fileName.LastIndexOf('.')))
                        {
                            File.Move(path, path.Replace(oldName, newName));
                            await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                text: $"Audio Meme changed the name from *{oldName}* to *{newName}*", parseMode: ParseMode.Markdown);
                            return;
                        }
                    }
                }

                else if (e.Message.Text.Contains("Picture", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (var path in Directory.GetFiles(@$"Files/Photos"))
                    {
                        string fileName = Path.GetFileName(path);

                        if (oldName == fileName.Substring(0, fileName.LastIndexOf('.')))
                        {
                            File.Move(path, path.Replace(oldName, newName));
                            await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                text: $"Picture Meme changed the name from *{oldName}* to *{newName}*", parseMode: ParseMode.Markdown);
                            return;
                        }
                    }
                }

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Failed replacement.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
