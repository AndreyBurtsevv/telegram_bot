using Serilog;
using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Constants;

namespace TelegramMemBot.Services
{
    public class AdminService
    {
        public TelegramBotClient Bot { get; private set; }
        public static string NameNevFile { get; set; }

        public AdminService(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public static bool IsAdmin(Chat chat, User user)
        {
            if (chat.Id == TelegramSettings.AdminId && user.Id == TelegramSettings.AdminId && user.Username == TelegramSettings.AdminName && user.FirstName == TelegramSettings.AdminFirstName)
                return true;

            else return false;
        }

        public async void DownloadNewMem(MessageEventArgs msgEvent)
        {
            try
            {
                if (string.IsNullOrEmpty(NameNevFile))
                    return;

                if (msgEvent.Message.Type == MessageType.Photo)
                {
                    var file = Bot.GetFileAsync(msgEvent.Message.Photo.Last().FileId).Result;
                    var extension = Path.GetExtension(file.FilePath);

                    using FileStream fs = new(@$"Files/Photos/{NameNevFile}{extension}", FileMode.Create);
                    await Bot.DownloadFileAsync(file.FilePath, fs);
                }
                else if (msgEvent.Message.Type == MessageType.Audio)
                {
                    var file = Bot.GetFileAsync(msgEvent.Message.Audio.FileId).Result;
                    var extension = Path.GetExtension(file.FilePath);

                    using FileStream fs = new(@$"Files/Audios/{NameNevFile}{extension}", FileMode.Create);
                    await Bot.DownloadFileAsync(file.FilePath, fs);
                }

                NameNevFile = null;
                await Bot.SendTextMessageAsync(chatId: msgEvent.Message.Chat, text: "The new meme is ready, dear Admin.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                NameNevFile = null;
                return;
            }
        }
    }
}
