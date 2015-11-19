using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.Model
{
    public class StatisticsModel
    {
        public string X { get; set; }
        public double Y { get; set; }

        public StatisticsModel(string x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
