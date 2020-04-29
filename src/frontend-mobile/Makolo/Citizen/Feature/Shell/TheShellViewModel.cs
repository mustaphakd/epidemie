using Citizen.Framework;
using Citizen.Models;
//using Citizen.Providers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable CC0037 // Remove commented code.

// using System.Windows.Input;
namespace Citizen.Feature.Shell
#pragma warning restore CC0037 // Remove commented code.
{
    internal class TheShellViewModel : BaseViewModel
    {
        public TheShellViewModel()
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            //Xamarin.Forms.Routing.RegisterRoute(Core.Routes.Products, typeof(Mobile.Feature.Products.Landing));
            //Xamarin.Forms.Routing.RegisterRoute(Core.Routes.Articles, typeof(Mobile.Feature.Articles.Landing));
           // Xamarin.Forms.Routing.RegisterRoute(Core.Routes.Categories, typeof(Mobile.Feature.Categories.Landing));
        }
        /*public ICommand PhotoCommand => new AsyncCommand(
   _ => App.NavigateModallyToAsync(new CameraPreviewTakePhotoPage(), animated: false));

public ICommand ARCommand => new AsyncCommand(
   _ => App.NavigateToAsync(new CameraPreviewPage(), closeFlyout: true));

public ICommand LogOutCommand => new AsyncCommand(_ => App.NavigateModallyToAsync(new LogInPage()));

public ICommand ProductTypeCommand => new AsyncCommand(
   typeId => App.NavigateToAsync(new ProductCategoryPage(typeId as string), closeFlyout: true));

public ICommand ProfileCommand => FeatureNotAvailableCommand;

public ICommand SettingsCommand => new AsyncCommand(
   _ => App.NavigateToAsync(new SettingsPage(), closeFlyout: true)); */


        //public ObservableCollection<MenuItem> MenuItems => DependencyService.Get<MenuProvider>().MenuItems;
        //public ICommand ViewFeedCommand => new AsyncCommand(
        //  async feed =>
        //    MessagingCenter.Send<Object, string>(this, Messaging.Tokens.ViewFeed, feed as string)); //App.NavigateToAsync(new FeedPage(feed as string ), closeFlyout: true)
    }
}