using System;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.XForms.PopupLayout;

namespace Citizen.Feature.Shell
{

   [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TheShell : Xamarin.Forms.Shell
    {
        public static readonly TimeSpan TimeFlyoutCloses = TimeSpan.FromSeconds(0.5f);
        SfPopupLayout popupLayout;

        public TheShell()
        {
            InitializeComponent();
            BindingContext = new TheShellViewModel();
            popupLayout = new SfPopupLayout();
        }

        internal async Task CloseFlyoutAsync()
        {
            FlyoutIsPresented = false;
            await Task.Delay(TimeFlyoutCloses);
        }

        /*internal void GoToAsync(string pagePath)
        {
            var element = Xamarin.Forms.Routing.GetOrCreateContent(pagePath) as Xamarin.Forms.ContentPage;
            var shellSection = new ShellSection();
            var shellContent = new ShellContent { Content = element };
            shellSection.Items.Add(shellContent);
            shellSection.CurrentItem = shellContent;

            this.Items.Add(shellSection);
            this.CurrentItem = this.Items[this.Items.Count - 1];
        } */
        internal void AppendToSection(string pagePath)
        {
            var element = Xamarin.Forms.Routing.GetOrCreateContent(pagePath) as Xamarin.Forms.ContentPage;
            /*var shellSection = new ShellSection();
            var shellContent = new ShellContent { Content = element };
            shellSection.Items.Add(shellContent);
            shellSection.CurrentItem = shellContent;*/

            var shellItem = this.Items[0];
            /*shellItem.Items.Add(shellSection);
            shellSection.Parent = shellItem;
            shellItem.CurrentItem = shellSection;*/

            var shellContent = shellItem.CurrentItem.CurrentItem;
            shellContent.Navigation.PushAsync(element);
        }

        internal void ReplaceRootContent(string pagePath)
        {
            
            var element = Xamarin.Forms.Routing.GetOrCreateContent(pagePath) as Xamarin.Forms.ContentPage;
            this.Items[0].Parent = null;
            this.Items.Clear();

            var shellSection = new ShellSection();
            var shellContent = new ShellContent { Content = element };
            element.Parent = shellContent;

            shellSection.Items.Add(shellContent);
            shellContent.Parent = shellSection;
            shellSection.CurrentItem = shellContent;

            var shellItem = new ShellItem();
            shellItem.Items.Add(shellSection);
            shellSection.Parent = shellItem;
            shellItem.CurrentItem = shellSection;

            this.Items.Add(shellItem);

            this.CurrentItem = this.Items[0];
        }

        internal void SetRoot(ShellItem shellItem)
        {
            if (popupLayout.Content != null)
            {
                popupLayout.Content.BindingContext = null;
            }
            
            popupLayout.Content = null;

            if (this.Items.Count > 0)
            {
               // this.Items[0].Parent = null;

                if (this.Items[0].CurrentItem.CurrentItem != null)
                {
                    this.Items[0].CurrentItem.CurrentItem.BindingContext = null;
                    this.Items[0].CurrentItem.CurrentItem.Content = null;
                    this.Items[0].CurrentItem.CurrentItem.Parent = null;
                }

                this.Items[0].CurrentItem.BindingContext = null;
                this.Items[0].CurrentItem.Parent = null;

                try
                {
                    this.Items[0].CurrentItem.CurrentItem = null;
                }
                catch(Exception)
                {

                }

                try
                {
                    this.Items[0].CurrentItem = null;
                }
                catch (Exception){ }


                this.Items[0].Items.Clear();
                this.Items.RemoveAt(0);
                this.Items.Clear();
                // Task.Delay(700);
            }
            else
            {

                this.Items.Clear();
            }

            popupLayout = new SfPopupLayout();
            var page = (Xamarin.Forms.ContentPage)shellItem.CurrentItem.CurrentItem.Content;
            popupLayout.Content = null;
            page.Content.Parent = null;

            popupLayout.Content = page.Content;
            popupLayout.Content.Parent = popupLayout;

            page.Content = popupLayout;
            popupLayout.Parent = page;

            this.Items.Add(shellItem);
            this.CurrentItem = shellItem;
        }

        internal void ShowMessage(string message)
        {
            var view = popupLayout.PopupView;
            view.HeaderTitle = " Message";
            view.ShowHeader = true;
            view.ShowCloseButton = true;
            view.AcceptButtonText = String.Empty;
            view.ContentTemplate = new DataTemplate( () => new Label { 
                Text = message, 
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            });
            popupLayout.Show();
        }
    }
}
