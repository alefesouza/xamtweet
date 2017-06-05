using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace XamTweet.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        const string UserIdKey = "userId";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "autoken";
        static readonly string AuthTokenDefault = string.Empty;

        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserIdKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
    }
}