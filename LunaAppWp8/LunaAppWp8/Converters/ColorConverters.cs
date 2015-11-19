using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPControls;
using WPControls.Models;

namespace LunaAppWp8.Converters
{
    public class ColorConverter : IDateToBrushConverter
    {
        public Brush Convert(DateTime dateTime, bool isSelected, PeriodDayTypeEnum dayType, Brush defaultValue, BrushType brushType)
        {
            string uri = string.Empty;
            WPControls.Models.PeriodDayTypeEnum periodDayType = dayType;
            double pastAndCurrentOpacity = 0.6;
            double futureOpacity = 0.6;
           // double futureOpacity = 0.25;

            if (brushType == BrushType.Background)
            {

                uri = string.Format("/Images/Dark/{0}{1}.png", periodDayType != WPControls.Models.PeriodDayTypeEnum.RegularDay ?
                    periodDayType.ToString() : string.Empty, isSelected ? "_selected" : dateTime == DateTime.Today? "_today": string.Empty);

                //if (dateTime == DateTime.Today)
                //    uri = string.Format("/Images/Dark/{0}{1}.png", periodDayType != WPControls.Models.PeriodDayTypeEnum.RegularDay ?
                //        periodDayType.ToString() : string.Empty, "_today");
                //else
                //    uri = string.Format("/Images/Dark/{0}{1}.png", periodDayType != WPControls.Models.PeriodDayTypeEnum.RegularDay ?
                //        periodDayType.ToString() : string.Empty, isSelected ? "_selected" : string.Empty);


                return new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(uri, UriKind.Relative)),
                    Opacity = dateTime > DateTime.Today ? futureOpacity : pastAndCurrentOpacity
                } as Brush;
            }
            else
            {
                SolidColorBrush highlightForeground = new SolidColorBrush(Color.FromArgb(255,191,82,121));

                if (dateTime == DateTime.Today)
                    return highlightForeground;
                    /* switch (periodDayType)
                    {
                       
                           
                        case WPControls.Models.PeriodDayTypeEnum.FertilityStartDay:
                        case WPControls.Models.PeriodDayTypeEnum.FertilityDay:
                        case WPControls.Models.PeriodDayTypeEnum.FertilityEndDay:
                            {
                                return highlightForeground;
                            }
                        case WPControls.Models.PeriodDayTypeEnum.CycleStartDay:
                        case WPControls.Models.PeriodDayTypeEnum.CycleDay:
                        case WPControls.Models.PeriodDayTypeEnum.CycleEndDay:
                            {
                                return highlightForeground;
                            }
                        case WPControls.Models.PeriodDayTypeEnum.RegularDay:
                        default:
                            return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)) as Brush;
                            
                    } * */
                else
                    return defaultValue;
            }
        }

    }
}