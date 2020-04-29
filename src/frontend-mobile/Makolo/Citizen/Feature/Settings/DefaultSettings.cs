using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Feature.Settings
{
    public class DefaultSettings
    {
        public const string ApiAuthorizationHeader = "Authorization";
        public static string RootApiUrl { get; set; } = "http://192.168.8.110:43567";

        public const string FeedsApiUrl = "/feeds";
        public const string SettingsStorage = "app_store";
        public const string AuthenticationApiUrl = "/auth";

        public const bool DebugMode =
        #if DEBUG
                    true;
        #else
                    false;
        #endif
    }
}
