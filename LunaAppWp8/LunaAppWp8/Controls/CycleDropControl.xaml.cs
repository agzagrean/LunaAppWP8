using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MonthlyCycleApp.Controls
{
    public partial class CycleDropControl : UserControl
    {
        #region Properties

        public string Today
        {
            get
            {
                return DateTime.Now.ToString("m"); 
            }
        }

        private int? daysToPeriod = 28;
        public int? DaysToPeriod
        {
            get
            {
                //do some computation
                return daysToPeriod;
            }
            set 
            {
                if (value != daysToPeriod)
                    daysToPeriod = value;
           
            }
        }

        public Visibility IsSetupCompleted
        {
            get 
            {
                return daysToPeriod.HasValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            }
        }

        #endregion
        public CycleDropControl()
        {
            InitializeComponent();
            this.DataContext = this;
           
        }

      
    }
}
