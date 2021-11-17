using System;

namespace TelegramMemBot.Constants
{
    public static class TelegramSettings
    {
        public static string Name { get; } = "EchoMemoBot";

        public static string Token { get; } = "_";

        public static TimeSpan Timeout { get; } = TimeSpan.FromSeconds(10);

        public static int AdminId { get; } = _;

        public static string AdminName { get; } = _;

        public static string AdminFirstName { get; } = _;
    }
}
