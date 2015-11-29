using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chat_App.Converter
{
    class ColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string colorName = string.Empty;
            if (value == null)
                return colorName;

            ParseUser currentUser = ParseUser.CurrentUser;
            if (currentUser == null)
                return colorName;

            if (currentUser.Username != (string)value)
            {
                colorName = "AntiqueWhite";
            }
            else
            {
                colorName = "LightGreen";
            }

            return colorName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
