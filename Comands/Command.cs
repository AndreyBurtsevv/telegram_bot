using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramMemBot.Comands
{
    abstract public class Command : BotCommand
    {
        public Command(string command, string description)
        {
            Command = command;
            Description = description;
        }

        public abstract void Action(object sender, MessageEventArgs e, ITelegramBotClient botClient);
    }
}
