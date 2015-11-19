using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LunaAppWp8.NotificationsAndTiles
{
    public enum RecurencePeriodType
    {
        None = 0,
        Yearly = 1,
        SixMonths = 2,
        ThreeMonths=3,
        Monthly = 4,
    }

    [DataContract]
    public class RecurencePeriod
    {
        [DataMember]
        public string ReccurenceName { get; set; }

        [DataMember]
        public RecurencePeriodType ReccurenceType { get; set; }

        [DataMember]
        public int ReccurenceValue { get; set; }

        public RecurencePeriod(RecurencePeriodType type, string name, int value)
        {
            ReccurenceType = type;
            ReccurenceName = name;
            ReccurenceValue = value;
        }
    }

  
}
