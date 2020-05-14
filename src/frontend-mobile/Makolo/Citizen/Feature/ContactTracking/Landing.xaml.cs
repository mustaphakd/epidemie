using Backend.Core.Types;
using Citizen.Core;
using Citizen.Extensions;
using Citizen.Framework;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.ContactTracking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Landing : BaseContentPage<LandingViewModel>
    {
        public Landing()
        {
            InitializeComponent();
            ConstructContactsTypes();
            ViewModel.OnDataReady += ViewModel_OnDataReady;
        }

        private bool processingContactsData = false;
        private void ViewModel_OnDataReady(object sender, EventArgs e)
        {
            processingContactsData = true;
            stepProgress.Children.Clear();

            foreach(var item in ViewModel.OrderedByDateContacts)
            {
                stepProgress.Children.Add(new StepView { PrimaryFormattedText = ViewModel.GetPrimaryText(item), SecondaryFormattedText = ViewModel.GetSecondaryText(item), Status = item.StepStatus });
            }

            processingContactsData = false;
            ResetPickerButtonText();
        }

        private void ConstructContactsTypes()
        {
            var pickerValues = ViewModel.ContactsKeyTranslation.Select(keypair => keypair.Value).ToList();
            picker.ItemsSource = pickerValues;
            ResetPickerButtonText();
        }

        private PhysicalContacts GetEnumValue(string translation)
        {
            var key = ViewModel.ContactsKeyTranslation.Single(keyPair => keyPair.Value == translation).Key;
            var enumVal = (PhysicalContacts)Enum.Parse(typeof(PhysicalContacts), key);

            return enumVal;
        }

        private void pickerBtn_Clicked(object sender, EventArgs e)
        {
           picker.IsOpen = true;
        }

        private void picker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (processingContactsData == true) { return; }

            
            var selection = e.NewValue.ToString();
            pickerBtn.Text = selection;
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            var pickerSelectedItemText = picker.SelectedItem.ToString().Trim();
            var buttonText = pickerBtn.Text.Trim();

            if (buttonText != pickerSelectedItemText)
            {
                return;
            }


            var viewModel = BindingContext as LandingViewModel;
            var contact = GetEnumValue(buttonText);
            var dateTime = DateTime.Now;


            var location = await Geolocation.GetLastKnownLocationAsync();

            var item  = viewModel.AddContact(contact, dateTime, location.Longitude.ToString(), location.Latitude.ToString());
            ResetPickerButtonText();
        }

        private void ResetPickerButtonText()
        {

            var translator = new TranslateExtension();
            translator.Text = "SelectContactType";
            var translation = translator.ProvideValue(null).ToString();
            pickerBtn.Text = translation;
        }
    }
}