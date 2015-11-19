using LunaAppWp8.NotificationsAndTiles;
using LunaAppWp8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.Helpers
{
    public static class Extensions
    {
        public static List<RecurencePeriod> RecurencePeriodToList()
        {
            List<RecurencePeriod> list = new List<RecurencePeriod>();

            list.Add(new RecurencePeriod(RecurencePeriodType.None, AppResources.None, 0));
            list.Add(new RecurencePeriod(RecurencePeriodType.Yearly, AppResources.Yearly, 12));
            list.Add(new RecurencePeriod(RecurencePeriodType.SixMonths, AppResources.SixMonths, 6));
            list.Add(new RecurencePeriod(RecurencePeriodType.ThreeMonths, AppResources.ThreeMonths, 3));
            list.Add(new RecurencePeriod(RecurencePeriodType.Monthly, AppResources.Monthly, 1));

            return list;
        }

        public static RecurencePeriod GetPeriod(this List<RecurencePeriod> periods, RecurencePeriodType type)
        {
            return periods.FirstOrDefault(x => x.ReccurenceType == type);
        }

     
    }
}
