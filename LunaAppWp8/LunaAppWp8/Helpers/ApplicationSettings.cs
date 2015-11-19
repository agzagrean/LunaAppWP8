using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.Helpers
{
    public static class ApplicationSettings
    {
        public const string APPLICATION_DEACTIVATED = "application_decativated";

        #region notifications
        public const string SHOW_ALARM_MENS_OVUL = "showalarmmenstruationandovulation";
        public const string IS_PILL_ALARM_ON = "ispillalarmon";
        public const string PILL_ALARM_TIME = "pillalarmtime";
        public const string IS_MENSTRUATION_ALARM_ON = "ismenstruationalarmon";
        public const string MENSTRUATION_ALARM_TIME = "menstruationalarmtime";
        public const string IS_OVULATION_ALARM_ON = "isovulationalarmon";
        public const string OVULATION_ALARM_TIME = "ovuilationalarmtime";
        public const string GYNE_CHECK_REMINDER_RECURENCE_TYPE = "gynereminderrectype";
        public const string BREAST_CHECK_REMINDER_RECURENCE_TYPE = "breastreminderrectype";

        public const string IS_PASSWORD_PROTECTED = "ispasswordprotected";
        public const string NEED_LOGIN = "needtologin";
        #endregion

        #region initial setup
        public const string CYCLE_DURATION_SETTING = "cycledurationsetting";
        public const string PERIOD_DURATION_SETTING = "perioddurationsetting";
        public const string LAST_PERIOD_SETTING = "lastperiodsetting";
        public const string SHOW_SETUP = "showsetup";
        #endregion

        #region Preferences 
        public const string FIRST_DAY_OF_WEEK = "firstdayofweek";
       
        public const string IS_PERIOD_FORECAST_ENABLED = "isperiodforecastenabled";
        public const string FERTILITY_FORECAST = "fertilityforecast";
        public const string IS_FERTILITY_FORECAST_ENABLED= "isfertilityforecastenabled";

        #endregion

        #region drop events
        public const string IS_CYCLE_START_CONFIRMED = "iscyclestartconfirmed";
        public const string IS_CYCLE_END_CONFIRMED = "iscycleendconfirmed";
        public const string SHOW_CYCLE_CONFIRMATION = "showcycleconfirmation";
        public const string NEED_TO_ADD_CYCLE_MANUALLY = "needtoaddcyclemanually";
        #endregion

        #region period steps
        public const string START_CYCLE_STEP = "startcyclestep";
        public const string END_CYCLE_STEP = "endcyclestep";
        #endregion

        #region updated cycle start and end
        public const string START_CYCLE_DATE = "startcycledate";
        public const string END_CYCLE_DATE = "endcycledate";
        #endregion


        public const int MINIM_CYCLE_PERIOD = 2;

        private static IsolatedStorageSettings isoStore;


        public static void SetIsoStore()
        {
            // Get the settings for this application.
            try
            {
                if (isoStore == null)
                {
                    if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
                        isoStore = IsolatedStorageSettings.ApplicationSettings;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public static T GetProperty<T>(string propertyName)
        {
            SetIsoStore();

            if (string.IsNullOrEmpty(propertyName)) return default(T);

            if (isoStore.Contains(propertyName))
            {
                var value = isoStore[propertyName];
                return (T)value;
            }
            return default(T);
        }

        public static T GetPropertyWithDefault<T>(string propertyName, object defaultValue)
        {
            SetIsoStore();

            if (string.IsNullOrEmpty(propertyName)) return (T)defaultValue;

            if (isoStore.Contains(propertyName))
            {
                var value = isoStore[propertyName];
                return (T)value;
            }
            return (T)defaultValue;
        }

        public static void SetProperty(string propertyName, object propertyValue)
        {
            SetIsoStore();

            // System.Diagnostics.Debug.WriteLine("******** SetProperty " + propertyName + ", " + propertyValue);
            if (string.IsNullOrEmpty(propertyName)
                || propertyValue == null) return;

            if (isoStore.Contains(propertyName))
            {
                isoStore[propertyName] = propertyValue;
            }
            else
            {
                isoStore.Add(propertyName, propertyValue);
            }
            isoStore.Save();
        }

        public static void RemoveProperty(string propertyName)
        {
            SetIsoStore();

            isoStore.Remove(propertyName);
        }
    }
}
