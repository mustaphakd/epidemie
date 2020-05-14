using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Citizen.Feature.Settings;
using Citizen.Framework.Files;
using Citizen.Models;
using Xamarin.Forms;

namespace Citizen.Services.Impl
{
    public class SettingsService : ISettingsService
    {
        private string key => DefaultSettings.SettingsStorage;
        public SettingsModel GetSettings()
        {
            SettingsModel model = null ; //= new SettingsModel { ServerEndpoint = DefaultSettings.RootApiUrl };

            if (App.Current.Properties.ContainsKey(key))
            {
                model = (SettingsModel)App.Current.Properties[key];
            }

            if (model == null)
            {
                try
                {
                    this.RetrieveModel(
                        (result) =>
                        {
                            model = result;
                        });
                }
                catch(Exception e)
                {
                    var test = e;
                    model = this.InitializeSettingModel();
                    this.registerSettingsModel(model);
                }
            }

            if (model == null)
            {
                model = this.InitializeSettingModel();
                this.registerSettingsModel(model);
            }

            return model;
        }

        private void RetrieveModel(Action<SettingsModel> modelSetter)
        {
            SettingsModel model = null;

            try
            {
                model = FileSystemManager.ReadLocalSetting<SettingsModel>(this.key);
            }
            catch(Exception ex)
            {
                DependencyService.Get<Infrastructure.ILoggingService>().Error(ex);
            }

            Core.CoreHelpers.ValidateDefined(model, "SettingsModel is required");

            modelSetter(model);
        }

        private async void registerSettingsModel(SettingsModel model)
        {
            await FileSystemManager.WriteLocalSetting<SettingsModel>(this.key, model);
            await this.SetSettings(model);
        }

        private SettingsModel InitializeSettingModel()
        {
            var model = new SettingsModel
            {
                DefaultConnectionString = "data.bin",
                SalesConnectionString = "sales.bin",
                Language = new Language { DisplayName = "English", ShortName = "en" },
                ServerEndpoint = DefaultSettings.RootApiUrl
            };

            return model;
        }

        public async Task SetSettings(SettingsModel model)
        {
            Core.CoreHelpers.ValidateDefined(model, $"{nameof(model)} is required");
            Application.Current.Properties[key] = model;
            await Application.Current.SavePropertiesAsync();

            //var restServices = DependencyService.Get<IRestServices>();
           // restServices.UpdateApiUrl(model.ServerEndpoint);
        }
    }
}
