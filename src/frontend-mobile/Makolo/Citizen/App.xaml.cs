using Citizen.Feature.Shell;
using Citizen.Infrastructure;
using Citizen.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Citizen
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        private static NavigableElement navigationRoot;

        public App()
        {
            InitializeComponent();
            RegisterServicesAndProviders();
            MainPage = new Citizen.Feature.Shell.TheShell();
        }

        public static NavigableElement NavigationRoot
        {
            get => GetShellSection(navigationRoot) ?? navigationRoot;
            set => navigationRoot = value;
        }

        public static TheShell Shell => Current.MainPage as TheShell;

        // It provides a navigatable section for elements which aren't explicitly defined within the Shell. For example,
        // ProductCategoryPage: it's accessed from the fly-out through a MenuItem but it doesn't belong to any section
        internal static ShellSection GetShellSection(Element element)
        {
            if (element == null)
            {
                return null;
            }

            var parent = element;
            var parentSection = parent as ShellSection;

            while (parentSection == null && parent != null)
            {
                parent = parent.Parent;
                parentSection = parent as ShellSection;
            }

            return parentSection;
        }
        internal static async Task NavigateBackAsync() => await NavigationRoot.Navigation.PopAsync();

        internal static async Task NavigateModallyBackAsync() => await NavigationRoot.Navigation.PopModalAsync();

        internal static async Task NavigateToAsync(Page page, bool closeFlyout = false)
        {
            if (closeFlyout)
            {
                await Shell.CloseFlyoutAsync();
            }

            await NavigationRoot.Navigation.PushAsync(page).ConfigureAwait(false);
        }

        internal static async Task NavigateModallyToAsync(Page page, bool animated = true)
        {
            await Shell.CloseFlyoutAsync();
            await NavigationRoot.Navigation.PushModalAsync(page, animated).ConfigureAwait(false);
        }

        /// <summary>
        ///  await Xamarin.Forms.Shell.Current.GoToAsync($"//articles?productId={Product.Id}&isnew={_initialState == ViewStates.Creating}");
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static async Task GotoAsync(string path)
        {
            await Xamarin.Forms.Shell.CurrentShell.GoToAsync(path);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void RegisterServicesAndProviders()
#pragma warning restore CC0091 // Use static method
        {

            //DependencyService.Register<MockDataStore>();
            DependencyService.Register<ILoggingService, DebugLoggingService>();

            DependencyService.Register<ILoggerFactory, Infrastructure.LoggerFactory>();
           // DependencyService.Register<SettingsService>(); 
            //DependencyService.Register<IDataStoreService, DataStoreService>();
            //DependencyService.Register<Services.Impl.DialogService>();
        }

    }
}
