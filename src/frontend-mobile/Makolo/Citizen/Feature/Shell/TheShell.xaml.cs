using System;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Shell
{

   [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TheShell
    {
        public static readonly TimeSpan TimeFlyoutCloses = TimeSpan.FromSeconds(0.5f);

        public TheShell()
        {
            //AdMaiora.RealXaml.Client.AppManager.Init(this);
            InitializeComponent();
            BindingContext = new TheShellViewModel();
        }

        internal async Task CloseFlyoutAsync()
        {
            FlyoutIsPresented = false;
            await Task.Delay(TimeFlyoutCloses);
        }
    }
}
