using Citizen.Framework.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Citizen.Converters
{
    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string path) || String.IsNullOrEmpty(path) || !(parameter is string storagePath))
            {
                var platform = (Xamarin.Forms.OnPlatform<string>)Application.Current.Resources["FontAwesomeRegular"];
                var color = (Color)Application.Current.Resources["SecondaryColor"];
                string family = platform;
                return new FontImageSource
                {
                    FontFamily = family,
                    Size = 25,
                    Glyph = Helpers.FontAwesomeIconFont.Circle,
                    Color = color
                };
            }

            var arr = storagePath.Split(Core.Constants.Separator.ToCharArray());
            var dirPath = FileSystemManager.ConstructFullPath(arr);
            var fullPath = FileSystemManager.Combine(dirPath, path);
            var source = ImageSource.FromFile(fullPath);
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
