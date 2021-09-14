using Serilog;
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramMemBot.Comands
{
    public static class CallbackQueryCommand
    {
        public static async void SendCallbackQuery(CallbackQueryEventArgs queryEvent, TelegramBotClient bot)
        {
            try
            {
                var message = queryEvent.CallbackQuery.Data;

                if (message[0] == 'I')
                    using (var streem = File.OpenRead(message.TrimStart('I')))
                        await bot.SendPhotoAsync(chatId: queryEvent.CallbackQuery.Message.Chat.Id, streem);
                else if (message[0] == 'A')
                {
                    var name = Path.GetFileName(message.TrimStart('A'));
                    name = name.Substring(0, name.LastIndexOf('.'));
                    using var streem = File.OpenRead(message.TrimStart('A'));
                    await bot.SendAudioAsync(chatId: queryEvent.CallbackQuery.Message.Chat.Id, streem, title: name);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }
    }
}
