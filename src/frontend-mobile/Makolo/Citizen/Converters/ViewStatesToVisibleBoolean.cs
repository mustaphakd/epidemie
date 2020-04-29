using Citizen.Core;
using Citizen.Framework.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Citizen.Converters
{
    public class ViewStatesToVisibleBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ViewStates viewState))
                return false;

            if ( parameter != null)
            {
                if (parameter is string parameterStr)
                {
                    var paramViewStates = new List<ViewStates>();
                    var parameterStrArr = parameterStr.Split(',');

                    Array.ForEach(
                        parameterStrArr,
                        (element) => {
                            var paramViewState = (ViewStates)Enum.Parse(typeof(ViewStates), parameterStr);
                            paramViewStates.Add(paramViewState);
                        });

                    if (paramViewStates.Contains(viewState))
                    {
                        return true;
                    }

                    return false;
                }
            }

            if (viewState == ViewStates.Deleting)
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
