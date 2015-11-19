using LunaAppWp8.Resources;
using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPControls.Models;

namespace LunaAppWp8.NotificationsAndTiles
{
    public class RemindersManager
    {

        public static Uri appUri = new Uri(AppResources.ApplicationInitialUri, UriKind.Relative);

        public static void AddMenstruationReminder(DateTime start)
        {
            AddOrReplaceReminder(
                AppResources.MenstruationReminderName,
                AppResources.ApplicationTitle,
                AppResources.MenstruationReminderContent,
                start.AddDays(-1),
                start,
                RecurrenceInterval.None,
                appUri);

            AddOrReplaceReminder(
               AppResources.MenstruationReminderName,
               AppResources.ApplicationTitle,
               AppResources.MenstruationReminderContent,
               start.AddDays(-1),
               start,
               RecurrenceInterval.None,
               appUri);
        }

        public static void AddOvulationReminder(DateTime start)
        {
            AddOrReplaceReminder(
                 AppResources.OvulationReminderName,
                 AppResources.ApplicationTitle,
                 AppResources.OvulationReminderContent,
                 start.AddDays(-1),
                 start,
                 RecurrenceInterval.None,
                 appUri);
        }

        public static void AddGyneReminder(DateTime beginTime, RecurencePeriod recPeriod)
        {
            AddMedicalCheckReminder(beginTime, recPeriod, AppResources.GyneControlReminderName, AppResources.GyneControlReminderContent);
        }

        private static void AddMedicalCheckReminder(DateTime beginTime, RecurencePeriod recPeriod, string reminderName,string reminderContent)
        {
            RecurrenceInterval interval = RecurrenceInterval.None;
            DateTime expirationTime = DateTime.MaxValue;
            bool needsToBeRemoved = false;

            switch (recPeriod.ReccurenceType)
            {
                case RecurencePeriodType.Yearly:
                    {
                        interval = RecurrenceInterval.Yearly;
                        beginTime = beginTime.AddYears(1);
                        break;
                    }
                case RecurencePeriodType.SixMonths:
                    {
                        expirationTime = beginTime.AddMonths(6);
                        interval = RecurrenceInterval.None;
                        beginTime = beginTime.AddMonths(6);
                        break;
                    }

                case RecurencePeriodType.ThreeMonths:
                    {
                        interval = RecurrenceInterval.None;
                        expirationTime = beginTime.AddMonths(3);
                        beginTime = beginTime.AddMonths(3);
                        break;
                    }
                case RecurencePeriodType.Monthly:
                    {
                        interval = RecurrenceInterval.Monthly;
                        beginTime = beginTime.AddMonths(1);
                        break;
                    }          
                case RecurencePeriodType.None:
                    {
                        needsToBeRemoved = true;
                        break;
                    }
            }
            if (needsToBeRemoved)
                RemoveAlarmOrReminder(reminderName);
            else
                AddOrReplaceReminder(
                        reminderName,
                        AppResources.ApplicationTitle,
                        string.Format(reminderContent, interval.ToString().ToLower()),
                        beginTime,
                        expirationTime,
                        interval,
                        appUri);
        }

        public static void AddBreastReminder(DateTime beginTime, RecurencePeriod recPeriod)
        {
            AddMedicalCheckReminder(beginTime, recPeriod, AppResources.BreastControlReminderName, AppResources.BreastControlReminderContent);
        }

        public static void AddPillAlarm(PeriodMonth period)
        {
            AddOrReplaceAlarm(
                AppResources.PillAlarmName,
                AppResources.PillAlarmContent,
                period.PeriodEndDay.AddDays(1),
                period.CycleEndDay,
                RecurrenceInterval.Daily,
                new Uri("/Reminder/Reminder01.wav", UriKind.Relative));
        }
    
        public static void AddPillAlarm(DateTime beginTime, DateTime endTime)
        {
            AddOrReplaceAlarm(
                AppResources.PillAlarmName,
                AppResources.PillAlarmContent,
                beginTime,
                endTime,
                RecurrenceInterval.Daily,
                new Uri("/Reminder/Reminder01.wav", UriKind.Relative));
        }

        public static void AddReminder(
                                        string name,
                                        string title,
                                        string content,
                                        DateTime beginTime,
                                        DateTime expirationTime,
                                        RecurrenceInterval recurrenceType,
                                        Uri navigationUri)
        {
            ScheduledAction action = ScheduledActionService.Find(name);
            if (action == null)
            {
                Reminder reminder = new Reminder(name);
                reminder.Title = title;
                reminder.Content = content;
                reminder.BeginTime = beginTime;
                reminder.ExpirationTime = expirationTime;
                reminder.RecurrenceType = recurrenceType;
                reminder.NavigationUri = navigationUri;
                // Register the reminder with the system.
                ScheduledActionService.Add(reminder);
            }
        }

        public static void AddOrReplaceReminder(
            string name,
            string title,
            string content,
            DateTime beginTime,
            DateTime expirationTime,
            RecurrenceInterval recurrenceType,
            Uri navigationUri)
        {
           
             Reminder reminder = new Reminder(name);
             reminder.Title = title;
             reminder.Content = content;
             reminder.BeginTime = beginTime;
             reminder.ExpirationTime = expirationTime;
             reminder.RecurrenceType = recurrenceType;
             reminder.NavigationUri = navigationUri;

             ScheduledAction action = ScheduledActionService.Find(name);
             if (action == null) 
                 // Register the reminder with the system.
                 ScheduledActionService.Add(reminder);
          //   else
          //       ScheduledActionService.Replace(reminder);
        }

        public static void AddOrReplaceAlarm(
            string name,
            string content,
            DateTime beginTime,
            DateTime expirationTime,
            RecurrenceInterval recurrenceType,
            Uri alarmSound ) 
        {
             ScheduledAction action = ScheduledActionService.Find(name);

             Alarm alarm = new Alarm(name);
             alarm.Content = content;
             alarm.Sound = alarmSound;
             alarm.BeginTime = beginTime;
             alarm.ExpirationTime = expirationTime;
             alarm.RecurrenceType = recurrenceType;

             if (action == null)
                 ScheduledActionService.Add(alarm);
             else
                 ScheduledActionService.Replace(alarm);
        }

        public static void RemoveAlarmOrReminder(string name)
        {
            ScheduledAction action = ScheduledActionService.Find(name);
            if (action != null)
                ScheduledActionService.Remove(action.Name);
        }
    }
}
