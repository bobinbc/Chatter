using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chat_App.Converter
{
    class AlignmentDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string alignmentName = string.Empty;

            if (value == null)
            {
                return alignmentName;
            }

            ParseUser currentUser = ParseUser.CurrentUser;
            if (currentUser == null)
                return alignmentName;

            if (currentUser.Username != (string)value)
            {
                alignmentName = "Left";
            }
            else
            {
                alignmentName = "Right";
            }

            return alignmentName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
