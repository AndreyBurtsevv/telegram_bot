using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramMemBot.Comands
{
    class RandomMemeCommand : Command
    {
        public RandomMemeCommand(string command = "Random Meme", string description = "Random Meme") : base(command, description) { }

        public async override void Action(object sender, MessageEventArgs mesgEvent, ITelegramBotClient botClient)
        {

            try
            {
                int num = new Random().Next(0, 6);

                using (var streemm = System.IO.File.OpenRead(@$"Files/Gifs/Gif{num}.gif"))
                {
                    InputMedia inputMedia = new(streemm, $"Gif{num}.gif");
                    await botClient.SendAnimationAsync(chatId: mesgEvent.Message.Chat, inputMedia, caption: "Wow wow! Looking for a Meme for you!", duration: 10);
                }

                await SendRandomMeme(mesgEvent, botClient);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }

        private static async Task SendRandomMeme(MessageEventArgs mesgEvent, ITelegramBotClient botClient)
        {
            Thread.Sleep(5000);
            await botClient.SendTextMessageAsync(chatId: mesgEvent.Message.Chat, text: "And so your meme:");
            if (new Random().Next(0, 2) == 0)
            {
                var list = Directory.GetFiles(@$"Files/Audios");
                var random = new Random().Next(0, list.Length);

                var name = Path.GetFileName(list[random]);
                name = name.Substring(0, name.LastIndexOf('.'));
                using var streem = System.IO.File.OpenRead(list[random]);
                await botClient.SendAudioAsync(chatId: mesgEvent.Message.Chat, streem, title: name);
            }
            else
            {
                var list = Directory.GetFiles(@$"Files/Photos");
                var random = new Random().Next(0, list.Length);
                using var streem = System.IO.File.OpenRead(list[random]);
                await botClient.SendPhotoAsync(chatId: mesgEvent.Message.Chat, streem);
            }
        }
    }
}
