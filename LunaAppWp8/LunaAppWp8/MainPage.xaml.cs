using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using LunaAppWp8.Resources;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using LunaAppWp8.Model;
using System.Windows.Data;
using WPControls.Models;
using LunaAppWp8.Helpers;
using WPControls.Helpers;
using LunaAppWp8.NotificationsAndTiles;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using LunaAppWp8;


namespace LunaAppWp8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.MainViewModel;
            Loaded += MainPage_Loaded;
            App.MainViewModel.Return = false;
                  
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            (dropControl.Resources["Blink"] as Storyboard).Begin();

            Cal.PeriodCalendarProperty = null;
            Cal.PeriodCalendarProperty = App.MainViewModel.Calendar;

            //spChart is the chart name
            //spChart.Visibility = Visibility.Visible;
            App.MainViewModel.StatisticsCollection = GenerateStatisticDatasDynamic();
            //spChart.DataContext = GenerateStatisticDatasDynamic();

           
        }
        public static ObservableCollection<StatisticsModel> GenerateStatisticDatas()
        {
            ObservableCollection<StatisticsModel> data = new ObservableCollection<StatisticsModel>();
            data.Add(new StatisticsModel("Data 0", 1));
            data.Add(new StatisticsModel("Data 1", 2));
            data.Add(new StatisticsModel("Data 2", 3));
            data.Add(new StatisticsModel("Data 3", 4));
            return data;

        }

        public static ObservableCollection<StatisticsModel> GenerateStatisticDatasDynamic()
        {
            var periods = App.MainViewModel.Calendar.PastPeriods;
            ObservableCollection<StatisticsModel> data = new ObservableCollection<StatisticsModel>();
            foreach (PeriodMonth per in periods)
            {
                DateTime startDate = per.PeriodStartDay;
                //startDate.ToString("MMM", CultureInfo.InvariantCulture)
                data.Add(new StatisticsModel(string.Format("{0} {1}", startDate.Month, startDate.Year), per.CycleDuration));
            }

           
            return data;

        }
        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //remove initial setup page from history
            if (NavigationService.BackStack.Count() > 0)
            {
                var previousPage = this.NavigationService.BackStack.FirstOrDefault();

                if (previousPage != null && previousPage.Source.ToString().StartsWith("/InitialSetupPage.xaml"))
                {
                    this.NavigationService.RemoveBackEntry();
                }
            }


            if (App.MainViewModel.GyneCheckReminderPeriod != null)
            {
                expanderGyne.Header = App.MainViewModel.GyneCheckReminderPeriod.ReccurenceName;
            }
            else
            {
                App.MainViewModel.GyneCheckReminderPeriod = App.MainViewModel.GyneCheckPeriods.GetPeriod(RecurencePeriodType.None);
                expanderGyne.Header = App.MainViewModel.GyneCheckReminderPeriod.ReccurenceName;
            }

            if (App.MainViewModel.BreastCheckReminderPeriod != null)
            {
                expanderBreast.Header = App.MainViewModel.BreastCheckReminderPeriod.ReccurenceName;
            }
            else
            {
                App.MainViewModel.BreastCheckReminderPeriod = App.MainViewModel.BreastCheckPeriods.GetPeriod(RecurencePeriodType.None);
                expanderBreast.Header = App.MainViewModel.BreastCheckReminderPeriod.ReccurenceName;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
           // if (!outToTimePicker)
          //      FlipTile.CreateOrUpdateFlipTile(App.LunaViewModel.DaysToPeriod, App.LunaViewModel.DaysToPeriodText);
        }
     
        #endregion

        #region Events

        private void toggleBtnPillAllarm_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitchBtn = (sender as ToggleSwitch);
            Grid gridContainer = (toggleSwitchBtn.Parent as Grid).Children.OfType<Grid>().SingleOrDefault(x => x.Name == toggleSwitchBtn.Name + "TimePanel") as Grid;

            SetupAnimation(toggleSwitchBtn, gridContainer, 80);
        }
      
        private void panoramaControl_Loaded(object sender, RoutedEventArgs e)
        {
            panoramaControl.Visibility = Visibility.Visible;
        }

        //private void toggleBtnPeriodForecast_CheckedUnchecked(object sender, RoutedEventArgs e)
        //{
        //    ToggleSwitch toggleSwitchBtn = (sender as ToggleSwitch);
        //    TextBlock tblExplanation = (toggleSwitchBtn.Parent as Grid).Children.OfType<TextBlock>().SingleOrDefault(x => x.Name == toggleSwitchBtn.Name + "Explanation") as TextBlock;

        //    if (toggleSwitchBtn.IsChecked.GetValueOrDefault())
        //    {
        //        toggleSwitchBtn.Content = AppResources.Advanced;
        //    }
        //    else
        //    {
        //        toggleSwitchBtn.Content = AppResources.Standard;
        //    }
        //    /*
        //    Storyboard sbUp;
        //    Storyboard sbDown;

        //    if (gridContainer != null)
        //    {
        //        double height =  80;
        //        DefineAnimations(gridContainer, height, out sbUp, out sbDown);
        //        SetupCheckUncheckBehaviour(toggleSwitchBtn,  sbUp, sbDown, AppResources.Advanced, AppResources.Standard);

        //       if (tblExplanation != null)
        //           tblExplanation.Visibility = System.Windows.Visibility.Visible;
        //    }

        //    //settingsScroll.UpdateLayout();
        //    //settingsScroll.ScrollToVerticalOffset(SettingsGrid.ActualHeight);
        //     * */
        //}
        #endregion

        #region Private Methods

        private void AppBarContextualMenu()
        {
          //  cycleDropControl.Opacity = 0.7;
            this.ApplicationBar.Mode = ApplicationBarMode.Default;
            this.ApplicationBar.Opacity = 1;
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = true;
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).IsEnabled = App.MainViewModel.EndOfCycleEnabled;
        }

        private void RestoreAppBarDefaultValues()
        {
         //   cycleDropControl.Opacity = 1;
            this.ApplicationBar.Opacity = 0.5;
            this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = false;
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).IsEnabled = false;
        }
        #endregion

        #region Animation
        private static void SetupCheckUncheckBehaviour(ToggleSwitch toggleSwitchBtn, Storyboard sbUp, Storyboard sbDown, string onValue, string offValue)
        {
            TextBlock tblExplanation = (toggleSwitchBtn.Parent as Grid).Children.OfType<TextBlock>().SingleOrDefault(x => x.Name == toggleSwitchBtn.Name + "Explanation") as TextBlock;

            //is checked
            if (toggleSwitchBtn.IsChecked.GetValueOrDefault())
            {
                toggleSwitchBtn.Content = onValue;
                sbDown.Stop();
                sbUp.Begin();
                if (tblExplanation != null)
                    tblExplanation.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                toggleSwitchBtn.Content = offValue;
                sbUp.Stop();
                sbDown.Begin();
                if (tblExplanation != null)
                    tblExplanation.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private static void DefineAnimations(Grid gridContainer, double height, out Storyboard sbUp, out Storyboard sbDown)
        {
            sbUp = new Storyboard();
            sbUp.Duration = new Duration(new TimeSpan(0, 0, 1));
            sbDown = new Storyboard();
            sbDown.Duration = new Duration(new TimeSpan(0, 0, 1));


            DoubleAnimation heightUp = new DoubleAnimation()
            {
                From = 0,
                To = height,
                SpeedRatio = 4,
                Duration = new Duration(new TimeSpan(0, 0, 1)),

            };

            DoubleAnimation heightDown = new DoubleAnimation()
            {
                From = height,
                To = 0,
                SpeedRatio = 4,
                Duration = new Duration(new TimeSpan(0, 0, 1)),

            };
            Storyboard.SetTarget(heightUp, gridContainer);
            Storyboard.SetTarget(heightDown, gridContainer);

            Storyboard.SetTargetProperty(heightUp, new PropertyPath("Height"));
            Storyboard.SetTargetProperty(heightDown, new PropertyPath("Height"));

            sbUp.Children.Add(heightUp);
            sbDown.Children.Add(heightDown);
        }

        private static void SetupAnimation(ToggleSwitch toggleSwitchBtn, Grid gridContainer, double height)
        {
            Storyboard sbUp;
            Storyboard sbDown;
            if (gridContainer != null)
            {
                DefineAnimations(gridContainer, height, out sbUp, out sbDown);
                SetupCheckUncheckBehaviour(toggleSwitchBtn, sbUp, sbDown, AppResources.On, AppResources.Off);
            }
        }

        private static void AnimateValidation(TextBox tb)
        {
            Grid gridContainer = tb.Parent as Grid;
            Storyboard sbUp;
            Storyboard sbDown;
            if (gridContainer != null)
            {
                DefineAnimations(gridContainer, 110, out sbUp, out sbDown);
            }
        }
        #endregion

        #region Dialog events

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.Equals(AppResources.OkButton))
            {
                App.MainViewModel.OkCommand();

                Cal.PeriodCalendarProperty = null;
                Cal.PeriodCalendarProperty = App.MainViewModel.Calendar;
                Cal.Refresh();
            }

            if ((sender as Button).Content.Equals(AppResources.ReplaceButton))
            {
                App.MainViewModel.ReplaceCommand();
            }

            (dropControl.Resources["Blink"] as Storyboard).Resume();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.Equals(AppResources.CancelButton))
            {
                App.MainViewModel.CancelCommand();
            }
         
            (dropControl.Resources["Blink"] as Storyboard).Resume();
        }
        
        private void pkEndDateCycle_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            App.MainViewModel.Return = true;
        }

        private void pkEndDateCycle_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != DateTime.MinValue && 
                e.NewDateTime != e.OldDateTime &&
                e.NewDateTime.HasValue && 
                (sender as DatePicker).Name == pkEndDateCycle.Name)
            {
                DateTime tempEnd = e.NewDateTime.Value;
                bool validateEndDate = tempEnd < App.MainViewModel.SelectedStartCycle.AddDays(2);

                ValidationEnum validationType = ValidationEnum.NoNeedForValidation;

                if (tempEnd < App.MainViewModel.SelectedStartCycle)
                    validationType = ValidationEnum.EndDateBeforeStart;
                else
                    if (tempEnd < App.MainViewModel.SelectedStartCycle.AddDays(2))
                        validationType = ValidationEnum.EndDateBeforeStart;
                    else
                        //faaar in the future
                        if (Math.Abs((tempEnd - App.MainViewModel.SelectedEndCycle).Days) > App.MainViewModel.Calendar.AveragePeriodDuration)
                            validationType = ValidationEnum.EndDateFarInTheFuture;
             
                var period = App.MainViewModel.NextPeriod;
              
                if (validationType == ValidationEnum.NoNeedForValidation)
                {
                    App.MainViewModel.SelectedEndCycle = tempEnd;
                    App.MainViewModel.SetupDialog(validationType, period);
                }
                else
                {
                    App.MainViewModel.SelectedEndCycle = tempEnd;
                    App.MainViewModel.SetupDialog(validationType, period);
                }

                (sender as DatePicker).BorderBrush = (validationType == ValidationEnum.NoNeedForValidation) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
                forthRowText.Foreground = (validationType == ValidationEnum.NoNeedForValidation) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
            }
        }

        private void pkStartDateCycle_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime != DateTime.MinValue && 
                e.NewDateTime != e.OldDateTime && 
                e.NewDateTime.HasValue &&
                (sender as DatePicker).Name == pkStartDateCycle.Name)
            {
                DateTime tempStart = e.NewDateTime.Value;
                DateTime tempEnd = e.NewDateTime.Value.AddDays(App.MainViewModel.Calendar.AveragePeriodDuration - 1);

                ValidationEnum validationType = ValidationEnum.NoNeedForValidation;

                if (DateTime.Today < tempStart)
                    validationType = ValidationEnum.StartDateInFuture;
                else
                {
                    PeriodMonth nearbyPeriod = ExtensionMethods.FindOverlappingExistingPeriod(tempStart, tempEnd,
                           App.MainViewModel.Calendar.PastPeriods, App.MainViewModel.NextPeriod);
                    if (nearbyPeriod != null)
                        validationType = ValidationEnum.DateOverlappsExistingPeriod;
                }

                var period = App.MainViewModel.NextPeriod;
                if (validationType == ValidationEnum.NoNeedForValidation)
                {
                    App.MainViewModel.SelectedStartCycle = tempStart;
                    App.MainViewModel.SelectedEndCycle = tempEnd;

                    App.MainViewModel.SetupDialog(validationType, period);
                }
                else
                    App.MainViewModel.SetupDialog(validationType, period);

                (sender as DatePicker).BorderBrush = (validationType == ValidationEnum.NoNeedForValidation) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
                secondRowText.Foreground = (validationType == ValidationEnum.NoNeedForValidation) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
                okBtn.IsEnabled = (validationType != ValidationEnum.StartDateInFuture);
            }
        }
        
    

        #endregion

        bool outToTimePicker = false;

        private void timePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            outToTimePicker = true;
        }

        private void timePicker_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            outToTimePicker = false;
        }

        private void expander_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int row = -1;
            Grid gridContainer = new Grid();
            if ((sender as ExpanderView).Name == "expanderGyne")
            {
                gridContainer = expanderGyne.Parent as Grid;
                row = 6;
            }
            else
                if ((sender as ExpanderView).Name == "expanderBreast")
                {
                    gridContainer = expanderBreast.Parent as Grid;
                    row = 7;
                }

            var height = gridContainer.RowDefinitions[row].Height;
            if (height == new GridLength(80))
                gridContainer.RowDefinitions[row].Height = new GridLength(200);
            else
                gridContainer.RowDefinitions[row].Height = new GridLength(80);
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void ShareByEmail(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string subject = string.Empty;
            string body = string.Empty; //accepts HTML
        
            EmailComposeTask mailMessage = new EmailComposeTask()
            {
                Subject = subject,
                Body = body,
            };

            mailMessage.Show();
        }

        private void CreateCharts()
        { 

        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            ShareLinkTask shareMessage = new ShareLinkTask()
           {
               Title = "Check this cool new app!",
               Message = "Luna is a menstrual app",
               LinkUri = new Uri("http://google.com")
           };
            shareMessage.Show();
        }

        private void expandFeaturesPermissions_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int row = -1;
            Grid gridContainer = new Grid();
            if ((sender as ExpanderView).Name == "expanderFeatures")
            {
                gridContainer = expanderFeatures.Parent as Grid;
                row = 1;
            }
            else
                if ((sender as ExpanderView).Name == "expanderPermissions")
                {
                    gridContainer = expanderPermissions.Parent as Grid;
                    row = 2;
                }

            var height = gridContainer.RowDefinitions[row].Height;
            if (height == new GridLength(40))
                gridContainer.RowDefinitions[row].Height = new GridLength();
            else
                gridContainer.RowDefinitions[row].Height = new GridLength(40);

            gridContainer.UpdateLayout();
        }
    }
}