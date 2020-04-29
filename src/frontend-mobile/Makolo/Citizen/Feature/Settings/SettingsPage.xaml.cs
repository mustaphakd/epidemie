using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}