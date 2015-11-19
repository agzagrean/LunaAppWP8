using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LunaAppWp8.Converters
{
    public class EnumConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DayOfWeek enumValue = default(DayOfWeek);

            enumValue = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), value.ToString());

            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {

        
            int returnValue = 0;

            returnValue = (int)Enum.Parse(typeof(DayOfWeek), value.ToString());

            return returnValue;
           
        }
    }
}
