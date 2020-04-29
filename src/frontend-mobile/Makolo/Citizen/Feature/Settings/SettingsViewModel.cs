using Citizen.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Citizen.Models;

namespace Citizen.Feature.Settings
{
    public class SettingsViewModel: BaseViewModel
    {
        private SettingsModel SettingsModel { get; set; }

        public SettingsViewModel()
        {
            this.SaveCommand = new AsyncCommand(this.SaveChanges, ()=> this.CanExecute);
            this.SettingsModel = new SettingsModel { ServerEndpoint = String.Empty, SomeText = String.Empty};
        }

        public String ServerEndpoint
        {
            get => this.SettingsModel.ServerEndpoint;
            set
            {
                this.SettingsModel.ServerEndpoint = value;
                this.CanExecute = true;
                this.RaisePropertyChanged(nameof(ServerEndpoint));
            }
        }

        public String SomeText
        {
            get => this.SettingsModel.SomeText;
            set
            {
                this.SettingsModel.SomeText = value;
                this.CanExecute = true;
                this.RaisePropertyChanged(nameof(SomeText));
            }
        }

        private Boolean canExecute_;
        public Boolean CanExecute
        {
            get => this.canExecute_;
            private set => this.SetAndRaisePropertyChanged(ref this.canExecute_, value);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; } = new AsyncCommand(Cancel);
        public override Task InitializeAsync()
        {
            this.ServerEndpoint = DefaultSettings.RootApiUrl;

            if (App.Current.Properties.Keys.Contains(DefaultSettings.SettingsStorage))
            {
                var model = SettingsService.GetSettings() ;
                this.SettingsModel.ServerEndpoint = model.ServerEndpoint;
                this.SomeText = model.SomeText;
            }

            return base.InitializeAsync();
        }

        public override async Task UninitializeAsync()
        {
            await base.UninitializeAsync();
        }

        protected async Task SaveChanges()
        {
            if (String.IsNullOrEmpty(this.ServerEndpoint)) return;

            this.CanExecute = false;
            await SettingsService.SetSettings(this.SettingsModel);
            await App.NavigateModallyBackAsync();
        }

        protected static async Task Cancel()
        {
            await App.NavigateModallyBackAsync();
        }
    }
}