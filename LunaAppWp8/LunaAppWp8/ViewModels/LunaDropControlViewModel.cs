using LunaAppWp8.Helpers;
using LunaAppWp8.Model;
using LunaAppWp8.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPControls.Models;
using LunaAppWp8;

namespace LunaAppWp8.ViewModels
{
    public class LunaDropControlViewModel : INotifyPropertyChanged
    {

        private const double MAX_HEIGHT = 385;
        private double top = 0;
        private double bottom = -303;


        #region Event handlers
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        #region Drop infos
        public string Today
        {
            get
            {
                return DateTime.Now.ToString("m");
            }
        }

        private int daysToPeriod;
        public int DaysToPeriod
        {
            get
            {
                return daysToPeriod;
            }
            set
            {
                if (value != daysToPeriod)
                {
                    daysToPeriod = value;
                    NotifyPropertyChanged("DaysToPeriod");
                }
            }
        }

        private string daysToPeriodText;
        public string DaysToPeriodText
        {
            get { return daysToPeriodText; }
            set
            {
                if (value != daysToPeriodText)
                {
                    daysToPeriodText = value;
                    NotifyPropertyChanged("DaysToPeriodText");
                }
            }
        }

        public Visibility IsSetupCompleted
        {
            get
            {
                //do some computing
                return System.Windows.Visibility.Visible;

            }
        }

        private double topMarginValue;
        public double TopMarginValue 
        {
            get { return topMarginValue; }
            set
            {
                if (value != topMarginValue)
                {
                    topMarginValue = value;
                    NotifyPropertyChanged("TopMarginValue");
                }
            }
        }
        
        private double bottomMarginValue;
        public double BottomMarginValue
        {
            get { return bottomMarginValue; }
            set
            {
                if (value != bottomMarginValue)
                {
                    bottomMarginValue = value;
                    NotifyPropertyChanged("BottomMarginValue");
                }
            }
        }

        private Thickness wavingGridMargins;
        public Thickness WavingGridMargins
        {
            get { return wavingGridMargins; }
            set
            {
                if (value != wavingGridMargins)
                {
                    wavingGridMargins = value;
                    NotifyPropertyChanged("WavingGridMargins");
                }
            }
        }


        #endregion

        public LunaDropControlViewModel()
        {
            SetDropValues(App.MainViewModel.NextPeriod);

            if (App.MainViewModel.StartCycleConfirmed && App.MainViewModel.EndCycleConfirmed)
            {
                App.MainViewModel.ShowSelectStartDay = true;
                App.MainViewModel.ShowSelectEndDay = true;
            }
        }

        public void SetDropValues(PeriodMonth currentPeriod)
        {
            // the current cycle's mestruation hasn't started yet
            if (DateTime.Today < currentPeriod.PeriodStartDay)
            {
                int remainingDays = ((TimeSpan)(currentPeriod.PeriodStartDay - DateTime.Today)).Days;

                DaysToPeriodText = AppResources.DaysToPeriodText;
                DaysToPeriod = Math.Abs(remainingDays);
            }
            else
            {
              // the current cycle's menstruation has ended
                if (DateTime.Today > currentPeriod.PeriodEndDay)
                {
                    PeriodMonth nextPeriod = App.MainViewModel.Calendar.FuturePeriods.FirstOrDefault();

                    int remainingDays = ((TimeSpan)(nextPeriod.PeriodStartDay - DateTime.Today)).Days;

                    DaysToPeriodText = AppResources.DaysToPeriodText;
                    DaysToPeriod = Math.Abs(remainingDays);
                }
                else
                    // you are during your current cycle's menstruation
                    if (DateTime.Today >= currentPeriod.PeriodStartDay && DateTime.Today <= currentPeriod.PeriodEndDay)
                    {
                        int daysUntilEndCycle = ((TimeSpan)(currentPeriod.PeriodEndDay - DateTime.Today)).Days + 1;
                        DaysToPeriodText = AppResources.DayOfPeriodText;
                        DaysToPeriod = Math.Abs(daysUntilEndCycle);
                    }
            }

            SetWaveHeigth(currentPeriod);
        }

        private void SetWaveHeigth(PeriodMonth currentPeriod)
        {
            double top = 0;
            //from begining to half of cycle you have 100%
            if (DateTime.Today >= currentPeriod.PeriodStartDay &&
                DateTime.Today <= currentPeriod.PeriodStartDay.AddDays(currentPeriod.PeriodDuration/2))
                top = 0;
            else
                //from half cycle to end, decreasing
                if (DateTime.Today > currentPeriod.PeriodStartDay.AddDays(currentPeriod.PeriodDuration / 2) &&
                    DateTime.Today <= currentPeriod.PeriodEndDay)
                {
                    double totalDaysSpan = (currentPeriod.PeriodEndDay - currentPeriod.PeriodStartDay.AddDays(currentPeriod.PeriodDuration / 2)).Days;
                    double percentage = Math.Round(MAX_HEIGHT / totalDaysSpan);
                    double remaining = (currentPeriod.PeriodEndDay - DateTime.Today).Days +1;
                    top = - percentage * remaining;
                }
                else
                //half duration to start cycle, increasing
                {
                    double totalDaysSpan = currentPeriod.PeriodDuration / 2;
                    if (DaysToPeriod < totalDaysSpan &&
                         DateTime.Today < currentPeriod.PeriodStartDay)
                    { 
                        double percentage = Math.Round(MAX_HEIGHT / totalDaysSpan);
                        double remaining = (DateTime.Today- currentPeriod.PeriodStartDay).Days + 1;
                        top = -percentage * (totalDaysSpan - remaining);
                    }
                }

            double bottom = -303 - top;

            TopMarginValue = top;
            BottomMarginValue = bottom;

            WavingGridMargins = new Thickness(-40, top, 40, bottom);
        }

    }
}
