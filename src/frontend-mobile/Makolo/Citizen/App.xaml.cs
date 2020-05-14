using Citizen.Feature.Shell;
using Citizen.Infrastructure;
using Citizen.Services;
using Citizen.Services.Impl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            InitConfiguration();
            RegisterServicesAndProviders();
            MainPage = new Citizen.Feature.Shell.TheShell();
        }

        private void InitConfiguration()
        {
            Akavache.Registrations.Start("Makolo");
        }

        public static NavigableElement NavigationRoot
        {
            get => GetShellSection(navigationRoot) ?? navigationRoot;
            set => navigationRoot = value;
        }

        public static TheShell Shell => Current.MainPage as TheShell;

        public static void DisplayMessage(string message)
        {
            Shell.ShowMessage(message);
        }

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
        //await Xamarin.Forms.Shell.CurrentShell.GoToAsync(path);
        //await Shell.Navigation.PopToRootAsync();
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () => {
                var element = Xamarin.Forms.Routing.GetOrCreateContent(path) as Xamarin.Forms.Page;
                await App.NavigateToAsync(element);
            });
        }

        internal static async Task SetAsRoot(string path)
        {
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                var element = Xamarin.Forms.Routing.GetOrCreateContent(path) as Xamarin.Forms.Page;
                 App.ReplaceRoot(element);
            });
        }

        internal static INavigation NavigationRootContainer
        {
            get
            {
                return App.NavigationRoot.Navigation;
            }
        }

        internal static async Task ReplaceRootContent(string path)
        {
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                //var element = Xamarin.Forms.Routing.GetOrCreateContent(path) as Xamarin.Forms.ContentPage;
                Shell.ReplaceRootContent(path);
            });
        }

        internal static async Task AppendToSection(string path)
        {
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                Shell.AppendToSection(path);
            });
        }

        

        internal static void ReplaceRoot(Page page)
        {
            var root = NavigationRoot.Navigation.NavigationStack.Count > 0 ? NavigationRoot.Navigation.NavigationStack[0] : null;

            if (root == null)
            {
                var shell =  new ShellContent { Content = page };
                page.Parent = shell;

                var section = new ShellSection();
                section.Items.Add(shell);
                section.CurrentItem = shell;
                shell.Parent = section;

                Shell.SetRoot(section);
                navigationRoot = shell;
                return;
            }
            NavigationRootContainer.InsertPageBefore(page, root);
            PopToRootAsync();
        }

        internal static async Task PopToRootAsync()
        {
            while (NavigationRootContainer.ModalStack.Count > 0)
            {
                await NavigationRootContainer.PopModalAsync(false);
            }
            while (NavigationRootContainer.NavigationStack.Count > 1)
            {
                await NavigationRootContainer.PopAsync(false);
            }
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
            DependencyService.Register<SettingsService>(); 
            DependencyService.Register<IDataStoreService, DataStoreService>();
            DependencyService.Register<Services.Impl.DialogService>();
            DependencyService.Register<IAuthenticationService, AuthenticationService>();
        }

    }
}
