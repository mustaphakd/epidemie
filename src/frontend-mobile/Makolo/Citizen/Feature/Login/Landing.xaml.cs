using Citizen.Extensions;
using Citizen.Feature.Login.ViewModels;
using Syncfusion.SfCarousel.XForms;
using Syncfusion.SfRotator.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class Landing : ContentPage
    {
        public Landing()
        {
            InitializeComponent();
            LoadCarouselContent();

            configureRegistration();
            configureMarital();
            configureBirthDate();
        }

        private void configureBirthDate()
        {

            var context = registerTbView.BindingContext as SignUpPageViewModel;
            
            if (context == null || context.BirthDate == null) { return; }

            var minimumYear = DateTime.Now.AddYears(-200).Year;
            var birthYear = context.BirthDate.Year;

            if (birthYear < minimumYear) { return; } //leave placeholder visible.

            pickerBtn.Text = context.BirthDate.ToString("yyyy-MM-dd");
        }

        private bool maritalConfigured = false;
        private SfRadioButton widowRadio;
        private SfRadioButton divorcedRadio;
        private SfRadioButton marriedRadio;
        private SfRadioButton singleRadio;
        private void configureMarital()
        {
            if (maritalConfigured == true) return;

            var groupKey = new SfRadioGroupKey();

            var translator = new TranslateExtension();
            translator.Text = "EnumMaritalSingle";

            this.singleRadio = new SfRadioButton { Text = (string)translator.ProvideValue(null) };
            singleRadio.GroupKey = groupKey;

            translator.Text = "EnumMaritalMarried";
            this.marriedRadio = new SfRadioButton { Text = (string)translator.ProvideValue(null) };
            marriedRadio.GroupKey = groupKey;

            translator.Text = "EnumMaritalDivorced";
            this.divorcedRadio = new SfRadioButton { Text = (string)translator.ProvideValue(null) };
            divorcedRadio.GroupKey = groupKey;

            translator.Text = "EnumMaritaWidow";
            this.widowRadio = new SfRadioButton { Text = (string)translator.ProvideValue(null) };
            widowRadio.GroupKey = groupKey;


            //var flex = new FlexLayout();
            flex.Wrap = FlexWrap.Wrap;
            flex.Direction = FlexDirection.Row;
            flex.JustifyContent = FlexJustify.SpaceBetween;

            flex.Children.Add(singleRadio);
            flex.Children.Add(marriedRadio);
            flex.Children.Add(divorcedRadio);
            flex.Children.Add(widowRadio);

            var context = registerTbView.BindingContext as SignUpPageViewModel;
            maritalConfigured = true;

            if (context == null) return;

            widowRadio.IsChecked = context.Marital == Backend.Core.Types.MaritalStatus.Widow;
            widowRadio.StateChanged += (src, arg) =>
            {
                if (widowRadio.IsChecked == true)
                {
                    context.Marital = Backend.Core.Types.MaritalStatus.Widow;
                }
            };

            divorcedRadio.IsChecked = context.Marital == Backend.Core.Types.MaritalStatus.Divorced;
            divorcedRadio.StateChanged += (src, arg) =>
            {
                if (divorcedRadio.IsChecked == true)
                {
                    context.Marital = Backend.Core.Types.MaritalStatus.Divorced;
                }
            };

            marriedRadio.IsChecked = context.Marital == Backend.Core.Types.MaritalStatus.Married;
            marriedRadio.StateChanged += (src, arg) =>
            {
                if (marriedRadio.IsChecked == true)
                {
                    context.Marital = Backend.Core.Types.MaritalStatus.Married;
                }
            };

            singleRadio.IsChecked = context.Marital == Backend.Core.Types.MaritalStatus.Single;
            singleRadio.StateChanged += (src, arg) =>
            {
                if (singleRadio.IsChecked == true)
                {
                    context.Marital = Backend.Core.Types.MaritalStatus.Single;
                }
            };
        }

        private void MariatStatRdgp_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var context = registerTbView.BindingContext as SignUpPageViewModel;

            if (widowRadio.IsChecked == true)
            {
                context.Marital = Backend.Core.Types.MaritalStatus.Widow;
            }
            else if (singleRadio.IsChecked == true)
            {
                context.Marital = Backend.Core.Types.MaritalStatus.Single;
            }
            else if (marriedRadio.IsChecked == true)
            {
                context.Marital = Backend.Core.Types.MaritalStatus.Married;
            }
            else if (divorcedRadio.IsChecked == true)
            {
                context.Marital = Backend.Core.Types.MaritalStatus.Divorced;
            }
        }

        private bool genderConfigured = false;
        private void configureRegistration()
        {
            if (genderConfigured == true) return;

            var collection = new List<string>();
            var translator = new TranslateExtension();
            translator.Text = "GenderMale";
            collection.Add((string)translator.ProvideValue(null));
            translator.Text = "GenderFemale";
            collection.Add((string)translator.ProvideValue(null));

            genderCb.ComboBoxSource = collection;
            genderConfigured = true;
        }


        private void LoadCarouselContent()
        {
            var imageResourceExtension = new ImageResourceExtension();
            imageResourceExtension.Source = "Citizen.Images.Donate.png";
            ObservableCollection<SfRotatorItem> carouselItems = new ObservableCollection<SfRotatorItem>();
            carouselItems.Add(new SfRotatorItem() { ItemContent = new Image() { Source = (ImageSource)imageResourceExtension.ProvideValue(null), Aspect = Aspect.AspectFit } });
            imageResourceExtension.Source = "Citizen.Images.Humannnn.png";
            carouselItems.Add(new SfRotatorItem() { ItemContent = new Image() { Source = (ImageSource)imageResourceExtension.ProvideValue(null), Aspect = Aspect.AspectFit } });
            imageResourceExtension.Source = "Citizen.Images.map.png";
            carouselItems.Add(new SfRotatorItem() { ItemContent = new Image() { Source = (ImageSource)imageResourceExtension.ProvideValue(null), Aspect = Aspect.AspectFit } });

            loginCarousel.ItemsSource = carouselItems;
        }

        private void pickerBtn_Clicked(object sender, EventArgs e)
        {
            BirthPicker.IsOpen = true;
        }

        private void BirthPicker_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
        {
            var context = registerTbView.BindingContext as SignUpPageViewModel;

            var minimumYear = DateTime.Now.AddYears(-200).Year;
            var birthYear =  BirthPicker.Date.Year;

            if (birthYear < minimumYear) { return; } //leave placeholder visible.

            context.BirthDate = BirthPicker.Date;
            pickerBtn.Text = context.BirthDate.ToString("yyyy-MM-dd");
            
        }
    }
}