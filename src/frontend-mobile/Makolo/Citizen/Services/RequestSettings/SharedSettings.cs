using Refit;
using Refit.Insane.PowerPack.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Citizen.Services.RequestSettings
{
    public static class SharedSettings
    {
        private static RefitSettings refitSettings;
        private static RestServiceBuilder restServiceBuilder;

        static SharedSettings()
        {
            refitSettings = new RefitSettings();
            restServiceBuilder = RestServiceBuilder
                .WithAutoRetry()
                .WithCaching()
                .WithRefitSettings(SharedSettings.RefitSettings);

            RestService = restServiceBuilder.BuildRestService(typeof(App).GetTypeInfo().Assembly);
        }

        public static RefitSettings RefitSettings
        {
            get
            {
                return refitSettings;
            }
        }
        public static RestServiceBuilder RestServiceBuilder
        {
            get
            {
                return restServiceBuilder;
            }
        }

        public static IRestService RestService { get; }

    }
}
