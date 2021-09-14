using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramMemBot.Comands;
using TelegramMemBot.Services;

namespace TelegramMemBot
{
    class Program
    {
        private static TelegramBotClient bot;
        private static AdminService adminService;

        static void Main()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@$"appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .Enrich.FromLogContext()
                    .WriteTo.File(@"Logs/log.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()
                    .CreateLogger();

            Log.Logger.Information($" | The application is working.");

            bot = BotService.Сlient;
            adminService = new AdminService(bot);

            bot.OnMessage += BotOnMessageReceived;

            bot.OnCallbackQuery += SendCallbackQuery;

            bot.StartReceiving();

            Thread.Sleep(Timeout.Infinite);

            Log.Logger.Error($" | The app died.");
        }

        private static async void SendCallbackQuery(object sender, CallbackQueryEventArgs queryEvent)
        {
            try
            {
                CallbackQueryCommand.SendCallbackQuery(queryEvent, bot);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception message: " + ex.Message + (ex.InnerException != null ? (". Inner exception: " + ex.InnerException.Message) : ""));
                return;
            }
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs msgEvent)
        {
            try
            {
                if (msgEvent.Message.Type == MessageType.Text)
                {
                    Log.Logger.Information($" | {msgEvent.Message.From.Username} | {msgEvent.Message.Text}");
                    BotService.Commands.FirstOrDefault(x => msgEvent.Message.Text.Contains(x.Command, StringComparison.CurrentCultureIgnoreCase))?.Action(sender, msgEvent, bot);
                }
                else if (msgEvent.Message.Type != MessageType.Text)
                {
                    Log.Logger.Information($" | {msgEvent.Message.From.Username} | Message type - {msgEvent.Message.Type}");
                    BotService.Error(msgEvent, bot);

                    if (AdminService.IsAdmin(msgEvent.Message.Chat, msgEvent.Message.From))
                    {
                        adminService.DownloadNewMem(msgEvent);
                    }
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
