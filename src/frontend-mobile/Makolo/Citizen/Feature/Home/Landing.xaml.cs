using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Landing : ContentPage
    {
        public Landing()
        {
            InitializeComponent();
        }

        private async void ContactTracking_Clicked(object sender, EventArgs e)
        {
            //todo
            //await App.GotoAsync(Core.Routes.PhysicalContacts);
            await App.AppendToSection(Core.Routes.PhysicalContacts);
        }
    }
}