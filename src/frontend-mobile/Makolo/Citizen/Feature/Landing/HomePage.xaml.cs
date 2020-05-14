using Citizen.Framework;
using Citizen.Feature.Landing;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : BaseContentPage<HomeViewModel>
    {
        public HomePage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            App.NavigationRoot = this;
            BindingContext = new HomeViewModel();
        }
    }
}
