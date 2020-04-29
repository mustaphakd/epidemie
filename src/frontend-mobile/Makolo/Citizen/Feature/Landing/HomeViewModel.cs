using Citizen;
using Citizen.Framework;
using Citizen.Feature.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Citizen.Feature.Landing
{
    public class HomeViewModel: BaseViewModel
    {
        //public const string LogInCompleted = nameof(LogInCompleted);
        public const string LoggingPrefix = "Mobile.Feature.Landing.HomeViewModel::";
        private Object syncObj = new Object();


        private Boolean canExecute_;
        public Boolean CanExecute {
            get => this.canExecute_;
            private set => this.SetAndRaisePropertyChanged(ref this.canExecute_, value);
        }

        private Boolean canSeetingsExecute_;
        public Boolean CanSettingsExecute
        {
            get => this.canSeetingsExecute_;
            private set => this.SetAndRaisePropertyChanged(ref this.canSeetingsExecute_, value);
        }

        public ICommand TapCommand => new AsyncCommand(this.TapAsync);
        public ICommand SettingsCommand => new AsyncCommand(this.SettingsAsync);

        public override async Task InitializeAsync()
        {
            LoggingService.Debug($"${LoggingPrefix}InitializeAsync() - Start");
            await base.InitializeAsync();
            LoggingService.Debug($"${LoggingPrefix}InitializeAsync() - End");
        }
        public override Task RenderedAsync()
        {
            LoggingService.Debug($"${LoggingPrefix}RenderedAsync() - Start");
            this.CanExecute = true;
            this.CanSettingsExecute = true;
            LoggingService.Debug($"${LoggingPrefix}RenderedAsync() - End");
            return Task.CompletedTask;
        }

        public override async Task UninitializeAsync()
        {
            LoggingService.Debug($"${LoggingPrefix}UninitializeAsync() - Start");

            await base.UninitializeAsync();
            LoggingService.Debug($"${LoggingPrefix}UninitializeAsync() - End");
        }


        private async Task TapAsync(Object argument)
        {
            LoggingService.Debug($"${LoggingPrefix}TapAsync() - Start");
            LoggingService.Debug($"argument: {argument}");

            if (! (argument is String feature))
            {
                LoggingService.Debug($"${LoggingPrefix}TapAsync() - End - Unkown object passed as argument to TapAsync: ${argument}");
                return; // Task.CompletedTask ;
            }
            /*
            switch(feature)
            {
                case Core.Constants.Analytics:
                    //return Xamarin.Forms.Shell.Current.GoToAsync(Core.Routes.Analytics) ;
                case Core.Constants.AddressBooks:
                    //return Xamarin.Forms.Shell.Current.GoToAsync(Core.Routes.AddressBooks);
                case Core.Constants.Products:
                    LoggingService.Debug($"${LoggingPrefix}TapAsync() - navigating to products route");
                    var page = new Feature.Products.Landing();
                    await App.NavigateToAsync(page, false);
                    //return Xamarin.Forms.Shell.Current.GoToAsync(Core.Routes.Products);
                    break;
                case Core.Constants.Shop:
                    //return Xamarin.Forms.Shell.Current.GoToAsync(Core.Routes.Shop);
                default:
                    throw new InvalidOperationException($"${LoggingPrefix}TapAsync() - End - Unkown feature requested: ${feature}");
            } */
        }

        private async Task SettingsAsync()
        {
            Task task = null;
            lock (this.syncObj)
            {
                if (this.CanSettingsExecute == false) return;

                this.CanSettingsExecute = false;
                LoggingService.Debug($"${LoggingPrefix}SettingsAsync() - Start");
                task = App.NavigateModallyToAsync(new SettingsPage());
                LoggingService.Debug($"${LoggingPrefix}SettingsAsync() - End");
            }

            if (task != null)
                await task;
        }
    }
}
