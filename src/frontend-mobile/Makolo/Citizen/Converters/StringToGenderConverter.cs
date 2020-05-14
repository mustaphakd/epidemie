using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Citizen.Controls;
using Citizen.Extensions;
using Backend.Core.Types;

namespace Citizen.Converters
{
    [Preserve(AllMembers = true)]
    public class StringToGenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var translator = new TranslateExtension();
            translator.Text = "GenderFemale";

            var gender = (Gender)value;


            if (gender == Gender.Female)
            {
                return translator.ProvideValue(null);
            }

            translator.Text = "GenderMale";
            return translator.ProvideValue(null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = "GenderFemale";

            if (String.IsNullOrEmpty(value.ToString()))
            {
                return Gender.Female;
            }

            var translator = new TranslateExtension();
            translator.Text = key ;

            var val = translator.ProvideValue(null);

            if (val == value)
            {
                return Gender.Female;
            }

            return Gender.Male;

        }

    }
}
