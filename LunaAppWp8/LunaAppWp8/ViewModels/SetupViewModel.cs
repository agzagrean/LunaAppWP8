using LunaAppWp8.Helpers;
using LunaAppWp8.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.ViewModels
{
    public class SetupViewModel :INotifyPropertyChanged
    {
        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        #region Persisted settings
        private string cycleDuration;
        public string CycleDuration
        {
            get
            {
                var storedValue = ApplicationSettings.GetProperty<string>(ApplicationSettings.CYCLE_DURATION_SETTING);
                return !string.IsNullOrWhiteSpace(cycleDuration) ?
                    cycleDuration : storedValue;
            }
            set
            {
                if (value != cycleDuration)
                {
                    cycleDuration = value;
                    ApplicationSettings.SetProperty(ApplicationSettings.CYCLE_DURATION_SETTING, cycleDuration);
                    NotifyPropertyChanged("CycleDuration");
                } 
            }
        }

        private string periodDuration;
        public string PeriodDuration
        {
            get
            {
                var storedValue = ApplicationSettings.GetProperty<string>(ApplicationSettings.PERIOD_DURATION_SETTING);
                return
                    !string.IsNullOrWhiteSpace(periodDuration) ?
                    periodDuration : storedValue;
            }
            set
            {
                if (value != periodDuration)
                {
                    periodDuration = value;
                    ApplicationSettings.SetProperty(ApplicationSettings.PERIOD_DURATION_SETTING, periodDuration);
                    NotifyPropertyChanged("PeriodDuration");
                }
            }
        }

        private DateTime lastPeriodDate;
        public DateTime LastPeriodDate
        { 
            get
            {
                var storedValue =ApplicationSettings.GetProperty<DateTime>(ApplicationSettings.LAST_PERIOD_SETTING);
                return
                    lastPeriodDate != DateTime.MinValue ?
                    lastPeriodDate :
                    storedValue;
            }
            set
            {
                if (value != lastPeriodDate)
                {
                    lastPeriodDate = value;
                    ApplicationSettings.SetProperty(ApplicationSettings.LAST_PERIOD_SETTING, lastPeriodDate);
                }
            } }

        private bool? showInitialSetup = null;
        public bool ShowInitialSetup
        {
            get
            {
                bool? storedValue= ApplicationSettings.GetProperty<bool?>(ApplicationSettings.SHOW_SETUP);
                return showInitialSetup.HasValue ? showInitialSetup.Value : storedValue.HasValue ? storedValue.Value : true;
            }
            set
            {
                if (value != showInitialSetup)
                {
                    showInitialSetup = value;
                    ApplicationSettings.SetProperty(ApplicationSettings.SHOW_SETUP, showInitialSetup);
                }
            }
        }

        #endregion

        private bool setupCompleted = false;
        public bool SetupCompleted
        {
            get
            {
                return setupCompleted;
            }
            set
            {
                if (value != setupCompleted)
                    setupCompleted = value;
            }
        }

        private bool lastDateSelected;
        public bool LastDateSelected
        {
            get 
            {
                return lastDateSelected;
            }
            set 
            {
                lastDateSelected = value;
                NotifyPropertyChanged("LastDateSelected");
            }
        }

        private string endSetupButtonText = AppResources.SetupIncompletedBtnText;
        public string EndSetupButtonText
        {
            get
            {
                return endSetupButtonText;
            }
            set
            {
                endSetupButtonText = value;
            }
        }

        #endregion

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                RefreshButton();
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
               
            }
        }

        public void RefreshButton()
        {
            SetupCompleted = !string.IsNullOrWhiteSpace(CycleDuration) &&
                         !string.IsNullOrWhiteSpace(PeriodDuration) &&
                         !CycleDuration.Equals("0") &&
                         !PeriodDuration.Equals("0") &&
                         LastPeriodDate != DateTime.MinValue &&
                         LastDateSelected;

            PropertyChanged(this,
                  new PropertyChangedEventArgs("SetupCompleted"));

            EndSetupButtonText = SetupCompleted ? AppResources.SetupCompletedBtnText : AppResources.SetupIncompletedBtnText;
            PropertyChanged(this,
                  new PropertyChangedEventArgs("EndSetupButtonText"));
        }

        public SetupViewModel()
        {
            LastDateSelected = !string.IsNullOrWhiteSpace(PeriodDuration) && !string.IsNullOrWhiteSpace(CycleDuration) && LastPeriodDate != DateTime.MinValue;

            if(string.IsNullOrWhiteSpace(PeriodDuration))
                    PeriodDuration = "6";
            if (string.IsNullOrWhiteSpace(CycleDuration))
                CycleDuration = "28";
            if (LastPeriodDate == DateTime.MinValue) 
                LastPeriodDate = DateTime.Today;
        }
    }
}
