using System;

namespace TelegramMemBot.Constants
{
    public static class TelegramSettings
    {
        public static string Name { get; } = "EchoMemoBot";

        public static string Token { get; } = "1679545088:AAEPC1deqM6eAUSUf5Lq5VLXboDKYfSFXGM";

        public static TimeSpan Timeout { get; } = TimeSpan.FromSeconds(10);

        public static int AdminId { get; } = 319746163;

        public static string AdminName { get; } = "big_mac_tape";

        public static string AdminFirstName { get; } = "ndr";
    }
}
