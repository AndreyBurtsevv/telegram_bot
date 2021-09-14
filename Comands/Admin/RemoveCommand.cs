using Serilog;
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Services;

namespace TelegramMemBot.Comands.Admin
{
    class RemoveCommand : Command
    {
        public RemoveCommand(string command = "Remove", string description = "Remove Meme") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient)
        {
            if (!AdminService.IsAdmin(e.Message.Chat, e.Message.From))
                return;

            try
            {
                string name = e.Message.Text[(e.Message.Text.IndexOf("Name:") + "Name:".Length)..].Trim();

                if (e.Message.Text.Contains("Audio", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (var path in Directory.GetFiles(@$"Files/Audios"))
                    {
                        string fileName = Path.GetFileName(path);
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

                        if (name == fileName)
                        {
                            File.Delete(path);
                            await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                text: $"Audio Meme *{fileName}* removed.", parseMode: ParseMode.Markdown);
                            return;
                        }
                    }
                }

                else if (e.Message.Text.Contains("Picture", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (var path in Directory.GetFiles(@$"Files/Photos"))
                    {
                        string fileName = Path.GetFileName(path);
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

                        if (name == fileName)
                        {
                            File.Delete(path);
                            await botClient.SendTextMessageAsync(chatId: e.Message.Chat,
                                text: $"Picture Meme *{fileName}* removed.", parseMode: ParseMode.Markdown);
                            return;
                        }
                    }
                }

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Failed remove.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}