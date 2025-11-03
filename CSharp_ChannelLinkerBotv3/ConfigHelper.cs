using System;
using System.IO;

namespace TemplateBot
{
    public static class ConfigHelper
    {
        // Base config directory - can be overridden by environment variable
        private static readonly string ConfigDirectory =
            Environment.GetEnvironmentVariable("CONFIG_DIR") ?? Path.Combine(Directory.GetCurrentDirectory(), "config");

        static ConfigHelper()
        {
            // Ensure config directory exists
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }
        }

        public static string GetConfigPath(string filename)
        {
            return Path.Combine(ConfigDirectory, filename);
        }

        public static string ChannelsLinkedPath => GetConfigPath("ChannelsLinked.json");
        public static string GuildSettingsListPath => GetConfigPath("GuildSettingsList.json");
        public static string MessagePrefixPath => GetConfigPath("MessagePrefix.json");
        public static string OwnersPath => GetConfigPath("owners.json");
        public static string TagsPath => GetConfigPath("tags.json");
        public static string EnvFilePath => GetConfigPath(".env");
    }
}
