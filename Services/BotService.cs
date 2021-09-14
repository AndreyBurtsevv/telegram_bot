using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMemBot.Comands;
using TelegramMemBot.Comands.Admin;
using TelegramMemBot.Constants;

namespace TelegramMemBot.Services
{
    public static class BotService
    {
        public static TelegramBotClient Сlient { get; private set; }
        public static List<Command> Commands { get; private set; }

        #region Initialization
        static BotService()
        {
            Commands = new List<Command>();

            Сlient = new TelegramBotClient(TelegramSettings.Token)
            {
                Timeout = TelegramSettings.Timeout
            };

            Сlient.SetMyCommandsAsync(CreateComands());
        }

        private static List<BotCommand> CreateComands()
        {
            Commands.Add(new PictureMemesCommand());
            Commands.Add(new AudioMemesCommand());
            Commands.Add(new StartCommand());
            Commands.Add(new FindByTemplateCommand());
            Commands.Add(new TemplateCommand());
            Commands.Add(new DownloadNewMemeCommand());
            Commands.Add(new NewMemCommand());
            Commands.Add(new ChangeMemeNameCommand());
            Commands.Add(new ReplacementCommand());
            Commands.Add(new DeleteMemeCommand());
            Commands.Add(new RemoveCommand());
            Commands.Add(new RandomMemeCommand());

            var mapper = new Mapper(
                new MapperConfiguration(cfg => cfg.CreateMap<BotCommand, Command>().ReverseMap())
                );

            return mapper.Map<List<Command>, List<BotCommand>>(Commands);
        }
        #endregion Initialization

        public static async void Error(MessageEventArgs e, ITelegramBotClient botClient)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "No-no-no...");
            }
            catch (Exception)
            {
                return;
            }
        }

        public static IReplyMarkup StartMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Audio Memes" }, new KeyboardButton { Text = "Picture Memes" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Find By Template" }, new KeyboardButton { Text = "Random Meme" } },
                },

                ResizeKeyboard = true,
            };
        }

        public static IReplyMarkup StartMenuForAdmin()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Audio Memes" }, new KeyboardButton { Text = "Picture Memes" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Find by Template" }, new KeyboardButton { Text = "Random Meme" }},
                    new List<KeyboardButton> { new KeyboardButton { Text = "Download new Meme" }, new KeyboardButton { Text = "Delete Meme" },  new KeyboardButton { Text = "Change Meme name" }},
                },

                ResizeKeyboard = true,
            };
        }

        public static InlineKeyboardMarkup ImagesButtons()
        {
            var list = new List<List<InlineKeyboardButton>>();

            foreach (var path in Directory.GetFiles(@$"Files/Photos"))
            {
                string name = Path.GetFileName(path);
                name = name.Substring(0, name.LastIndexOf('.'));
                list.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: name, callbackData: "I" + path) });
            }

            return new InlineKeyboardMarkup(list);
        }

        public static InlineKeyboardMarkup AudiosButtons()
        {
            var list = new List<List<InlineKeyboardButton>>();

            foreach (var path in Directory.GetFiles(@$"Files/Audios"))
            {
                string name = Path.GetFileName(path);
                name = name.Substring(0, name.LastIndexOf('.'));
                list.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: name, callbackData: "A" + path) });
            }

            return new InlineKeyboardMarkup(list);
        }
    }
}
